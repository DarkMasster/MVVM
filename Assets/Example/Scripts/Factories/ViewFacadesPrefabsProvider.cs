using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DM.MVVM.View;
using DM.Pooling;
using UnityEngine;

namespace DM.UI
{
	public class ViewFacadesPrefabsProvider : IPrefabsProvider<GameObject>
	{
		#region Private Fields
		private readonly Dictionary<string, GameObject> _cachedPrefabs = new(10);
		#endregion

		#region Interface Implementations
		public async UniTask<GameObject> Get(string key)
		{
			if (!_cachedPrefabs.TryGetValue(key, out var cachedPrefab))
			{
				var facadePrefab = LoadFacadePrefab(key);

				if (facadePrefab == null) throw new KeyNotResolvedException(typeof(ViewFacade), key);

				return facadePrefab;
			}

			return cachedPrefab;
		}

		public void Release(string key)
		{
			if (!_cachedPrefabs.TryGetValue(key, out var gameObject)) return;
			_cachedPrefabs.Remove(key);
		}
		#endregion

		#region Private Members
		private GameObject LoadFacadePrefab(string key) => Resources.Load<GameObject>($"Prefabs/{key}");
		#endregion
	}
}