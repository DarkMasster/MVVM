using System;
using System.Collections;
using System.Linq;
using Cysharp.Threading.Tasks;
using DM.TestUtils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DM.Pooling.Tests
{
	public class PoolingTests
	{
		#region Public Members
		[UnityTest]
		public IEnumerator GameObjectPoolGetterTest()
		{
			var gameObjectPool = CreateGameObjectPool();

			return PoolGetterTest(gameObjectPool).ToCoroutine();
		}

		[UnityTest]
		public IEnumerator GameObjectPoolWarmerTest()
		{
			var gameObjectPool = CreateGameObjectPool();

			return PoolWarmerTest(gameObjectPool).ToCoroutine();
		}

		[UnityTest]
		public IEnumerator GameObjectPoolInvalidAccessException()
		{
			var pool = CreateGameObjectPool();

			return PoolAccessException(pool).ToCoroutine();
		}

		[UnityTest]
		public IEnumerator GameObjectPoolWrongKeyException()
		{
			var pool = CreateGameObjectPool();

			return WrongKeyExceptionTest(pool).ToCoroutine();
		}

		[UnityTest]
		public IEnumerator WarmingExceptionTest()
		{
			var pool = CreateGameObjectPool();

			return PreWarmExceptionTest(pool).ToCoroutine();
		}

		[UnityTest]
		public IEnumerator ReleaseExceptionTest()
		{
			var pool = CreateGameObjectPool();

			return ReleaseExceptionTest(pool).ToCoroutine();
		}
		#endregion

		#region Private Members
		private async UniTask PoolGetterTest(IPool<GameObject> pool)
		{
			var key = "key";

			var go1 = await pool.Get(key);
			go1.transform.SetParent(null);

			var go2 = await pool.Get(key);
			go2.transform.SetParent(null);

			var go3 = await pool.Get(key);
			go3.transform.SetParent(null);

			var info = pool.PoolInfo;

			var trackedCount = info.TrackedObjectsStatistic.FirstOrDefault(p => p.Key == key);
			Assert.AreEqual(3, trackedCount.Capacity);

			pool.ReturnToPool(go1);
			info = pool.PoolInfo;
			trackedCount = info.TrackedObjectsStatistic.FirstOrDefault(p => p.Key == key);
			Assert.AreEqual(2, trackedCount.Capacity);

			var go = await pool.Get(key);
			pool.ReturnToPool(go);

			pool.ReturnToPool(go1);
			info = pool.PoolInfo;
			trackedCount = info.TrackedObjectsStatistic.FirstOrDefault(p => p.Key == key);
			Assert.AreEqual(2, trackedCount.Capacity);

			pool.ReturnToPool(go2);
			info = pool.PoolInfo;
			trackedCount = info.TrackedObjectsStatistic.FirstOrDefault(p => p.Key == key);
			Assert.AreEqual(1, trackedCount.Capacity);

			pool.ReturnToPool(go3);
			info = pool.PoolInfo;
			trackedCount = info.TrackedObjectsStatistic.FirstOrDefault(p => p.Key == key);
			Assert.AreEqual(null, trackedCount);
		}

		private async UniTask PoolWarmerTest(IPool<GameObject> pool)
		{
			var warmingParameters = CreatePrewarmParameters();
			var releaseParameters = CreateReleaseParameters();

			var previousProgressValue = 0.0f;

			var progress = Progress.Create((float value) =>
			{
				Assert.LessOrEqual(previousProgressValue, value);
				previousProgressValue = value;
			});

			var info = await pool.PreWarmPool(warmingParameters, progress);

			foreach (var parameter in warmingParameters)
			{
				var capacity = info.AvailableObjectsStatistic.First(p => p.Key == parameter.Key).Capacity;
				Assert.AreEqual(parameter.Capacity, capacity);
			}

			progress = Progress.Create((float value) =>
			{
				Assert.LessOrEqual(previousProgressValue, value);
				previousProgressValue = value;
			});

			info = await pool.ReleasePool(releaseParameters, progress);

			foreach (var parameter in releaseParameters)
			{
				if (warmingParameters.TryGetValue(parameter.Key, out var warmedCapacity))
				{
					var capacity = info.AvailableObjectsStatistic.First(p => p.Key == parameter.Key).Capacity;
					var expectedCapacity = warmedCapacity - parameter.Capacity;
					Assert.AreEqual(expectedCapacity, capacity);
				}
			}
		}

		private async UniTask PoolAccessException(IPool<GameObject> pool)
		{
			var prewarmKeys = CreatePrewarmParameters();
			pool.PreWarmPool(prewarmKeys).Forget();

			try
			{
				await pool.Get("key");
			}
			catch (Exception e)
			{
				Assert.Catch(() => throw e);
			}
		}

		private async UniTask WrongKeyExceptionTest(IPool<GameObject> pool)
		{
			try
			{
				await pool.Get("wrong-key");
			}
			catch (Exception e)
			{
				Assert.Catch(() => throw e);
			}
		}

		private async UniTask ReleaseExceptionTest(IPool<GameObject> pool)
		{
			var warmParameters = new PoolWarmingParameters { { "a", 1 } };
			var releaseParameters = CreateReleaseParameters();

			await pool.PreWarmPool(warmParameters);

			try
			{
				await pool.ReleasePool(releaseParameters);
			}
			catch (Exception e)
			{
				Assert.Catch(() => throw e);
			}

			var info = await pool.ReleasePool();

			var trackedCount = info.TrackedObjectsStatistic.FirstOrDefault(p => p.Key == "a");
			Assert.AreEqual(null, trackedCount);
		}

		private async UniTask PreWarmExceptionTest(IPool<GameObject> pool)
		{
			var warmingParameters = new PoolWarmingParameters { { "wrong-key", 1 } };

			try
			{
				await pool.PreWarmPool(warmingParameters);
			}
			catch (Exception e)
			{
				Assert.Catch(() => throw e);
			}
		}

		private PoolWarmingParameters CreatePrewarmParameters()
		{
			var prewarmKeys = new PoolWarmingParameters
			{
				{ "a", 1 },
				{ "b", 2 },
				{ "c", 3 }
			};

			return prewarmKeys;
		}

		private PoolWarmingParameters CreateReleaseParameters()
		{
			var releaseKeys = new PoolWarmingParameters
			{
				{ "a", 0 },
				{ "b", 1 },
				{ "c", 2 }
			};

			return releaseKeys;
		}

		private IPool<GameObject> CreateGameObjectPool()
		{
			var stashGo = new GameObject("Stash");

			var pool = new GameObjectsPool<MockGameObjectProvider, DefaultInstantiationProvider>(stashGo.transform);

			return pool;
		}
		#endregion

		#region Nested Types
		private class MockGameObjectProvider : IPrefabsProvider<GameObject>
		{
			#region Interface Implementations
			public async UniTask<GameObject> Get(string key)
			{
				await UniTask.Delay(1000);

				return key == "wrong-key" ? null : EditorTestUtils.GetFromAssets("Assets/Scripts/Pooling/Tests/Prefabs/Cube.prefab");
			}

			public void Release(string key)
			{
			}
			#endregion
		}
		#endregion
	}
}