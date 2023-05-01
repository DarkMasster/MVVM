using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DM.Pooling
{
	public class GameObjectsPool<TPrefabsProvider, TGameObjectFactory> : IPool<GameObject> 
		where TGameObjectFactory : IGameObjectFactory, new()
		where TPrefabsProvider : IPrefabsProvider<GameObject>, new()
	{
		#region Properties
		public PoolInfo PoolInfo => AssembleInfo();
		#endregion

		#region Private Fields
		private readonly IPrefabsProvider<GameObject> _prefabsProvider;

		private readonly IGameObjectFactory _gameObjectFactory;

		private readonly Transform _stash;

		private readonly Dictionary<string, Stack<GameObject>> _pooledGameObjects = new(10);

		private readonly Dictionary<string, GameObject> _prototypes = new(10);

		private readonly Dictionary<GameObject, string> _trackedObjects = new(10);

		private bool _isPoolWarming;
		#endregion

		#region Constructors
		public GameObjectsPool(Transform stash)
		{
			_prefabsProvider = new TPrefabsProvider();
			_gameObjectFactory = new TGameObjectFactory();
			_stash = stash;
			_stash.gameObject.SetActive(false);
		}
		#endregion

		#region Interface Implementations
		public async UniTask<PoolInfo> ReleasePool
			(IEnumerable<string> keys = null, IProgress<float> progress = null)
		{
			if (_isPoolWarming)
				throw new PoolInvalidAccessExceptioon();

			keys ??= _pooledGameObjects.Keys.ToArray();

			foreach (var key in keys)
			{
				if (_pooledGameObjects.TryGetValue(key, out var pool))
				{
					foreach (var go in pool) _gameObjectFactory.Destroy(go);

					_pooledGameObjects.Remove(key);
				}
				else
					throw new PoolReleasingException(key, new Exception());
			}

			progress?.Report(1.0f);
			var info = AssembleInfo();

			return info;
		}

		public async UniTask<GameObject> Get(string key)
		{
			if (_isPoolWarming)
				throw new PoolInvalidAccessExceptioon();

			GameObject go = null;

			if (_pooledGameObjects.TryGetValue(key, out var pool))
			{
				if (pool.Count > 0)
				{
					go = pool.Pop();
					_trackedObjects.Add(go, key);

					return go;
				}
			}

			try
			{
				var prototype = await GetPrototype(key);
				_prototypes.TryAdd(key, prototype);
				go = _gameObjectFactory.Instantiate(prototype, _stash);
				_trackedObjects.Add(go, key);
			}
			catch (Exception e)
			{
				throw new PoolKeyNotResolvedException(typeof(GameObject), key, e);
			}

			return go;
		}

		public void ReturnToPool(GameObject obj)
		{
			if (_isPoolWarming)
				throw new PoolInvalidAccessExceptioon();

			if (!_trackedObjects.TryGetValue(obj, out var key)) return;

			_trackedObjects.Remove(obj);
			AddToPool(key, obj);
		}

		public async UniTask<PoolInfo> PreWarmPool
			(PoolWarmingParameters parameters, IProgress<float> progress = null)
		{
			if (_isPoolWarming)
				throw new PoolInvalidAccessExceptioon();

			_isPoolWarming = true;

			var objectsAmountToPrewarm = CountObjectsAmountToPrewarm(parameters);
			var progressStep = 1.0f / (objectsAmountToPrewarm > 0 ? objectsAmountToPrewarm : 1.0f);
			var progressValue = 0.0f;

			var warmedKeys = new List<string>();

			foreach (var pair in parameters)
			{
				if (!_pooledGameObjects.TryGetValue(pair.Key, out var pool))
				{
					if (pair.Capacity > 0)
					{
						pool = new Stack<GameObject>(5);
						_pooledGameObjects.Add(pair.Key, pool);
					}
				}

				if (pair.Capacity <= 0) continue;

				GameObject prototype;

				try
				{
					prototype = await GetPrototype(pair.Key);
				}
				catch (Exception e)
				{
					var revertParameters = new PoolWarmingParameters();

					foreach (var key in warmedKeys)
					{
						if (parameters.TryGetValue(key, out var capacity))
							revertParameters.Add(key, capacity);
					}

					await ReleasePool(revertParameters);

					throw new PoolWarmingException(pair.Key, e);
				}

				_prototypes.TryAdd(pair.Key, prototype);

				while (pool != null && pool.Count < pair.Capacity)
				{
					var go = _gameObjectFactory.Instantiate(prototype, _stash);
					await UniTask.Yield();
					progressValue += progressStep;
					progress?.Report(progressValue);
					AddToPool(pair.Key, go);
				}

				warmedKeys.Add(pair.Key);
			}

			progress?.Report(1.0f);
			var info = AssembleInfo();

			_isPoolWarming = false;

			return info;
		}

		public async UniTask<PoolInfo> ReleasePool
			(PoolWarmingParameters parameters, IProgress<float> progress = null)
		{
			if (_isPoolWarming)
				throw new PoolInvalidAccessExceptioon();

			foreach (var pair in parameters)
			{
				if (!_pooledGameObjects.TryGetValue(pair.Key, out var pool))
					throw new PoolReleasingException(pair.Key, new Exception());

				var sourceCount = pool.Count;

				while (pool.Count > sourceCount - pair.Capacity && pool.Count > 0) _gameObjectFactory.Destroy(pool.Pop());

				if (pool.Count != 0) continue;
				_pooledGameObjects.Remove(pair.Key);
				_prototypes.Remove(pair.Key);
				_prefabsProvider.Release(pair.Key);
			}

			progress?.Report(1.0f);
			var info = AssembleInfo();

			return info;
		}
		#endregion

		#region Private Members
		private int CountObjectsAmountToPrewarm(PoolWarmingParameters parameters)
		{
			var objectsAmountToPrewarm = 0;

			foreach (var pair in parameters)
			{
				var objectsInPull = ObjectsAmountInPull(pair.Key);
				var prewarmForKey = pair.Capacity - objectsInPull;
				objectsAmountToPrewarm += prewarmForKey > 0 ? prewarmForKey : 0;
			}

			return objectsAmountToPrewarm;
		}

		private int ObjectsAmountInPull(string key) => _pooledGameObjects.TryGetValue(key, out var pool) ? pool.Count : 0;

		private async UniTask<GameObject> GetPrototype(string key)
		{
			if (_prototypes.TryGetValue(key, out var prototype)) return prototype;

			prototype = await _prefabsProvider.Get(key);

			if (!prototype)
				throw new Exception($"Prototype provider was called with key: {key} and return null.");

			return prototype;
		}

		private void AddToPool(string key, GameObject go)
		{
			go.transform.SetParent(_stash, true);

			if (_pooledGameObjects.TryGetValue(key, out var pool))
				pool.Push(go);
			else
			{
				pool = new Stack<GameObject>();
				pool.Push(go);
				_pooledGameObjects.Add(key, pool);
			}
		}

		private PoolInfo AssembleInfo()
		{
			var info = new PoolInfo
			{
				AvailableObjectsStatistic = _pooledGameObjects.Select(p => new KeyCapacityPair
				{
					Key = p.Key,
					Capacity = p.Value.Count
				}).ToList(),
				TrackedObjectsStatistic = _trackedObjects.GroupBy(p => p.Value).Select(g => new KeyCapacityPair
				{
					Key = g.Key,
					Capacity = g.Count()
				}).ToList()
			};

			return info;
		}
		#endregion
	}
}