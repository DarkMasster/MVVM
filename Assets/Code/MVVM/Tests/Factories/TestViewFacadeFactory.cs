using System.Collections.Generic;
using DM.MVVM.View;
using Cysharp.Threading.Tasks;
using MVVM.Tests.Polymorfic.View;
using UnityEditor;
using UnityEngine;

namespace DM.MVVM.Test
{
	public class TestViewFacadeFactory : IViewFacadeFactory
	{
		#region Private Fields
		private readonly Dictionary<string, string> _pathMap = new()
		{
			{ nameof(StaticViewFacade), "Assets/Scripts/MVVM/Tests/Prefabs/StaticViewFacade.prefab" },
			{ nameof(DynamicViewFacade), "Assets/Scripts/MVVM/Tests/Prefabs/DynamicViewFacade.prefab" },
			{ nameof(ItemViewFacade), "Assets/Scripts/MVVM/Tests/Prefabs/NestedViewFacade.prefab" },
			{ nameof(TestViewFacade), "Assets/Scripts/MVVM/Tests/Prefabs/TestView.prefab" },
			{ nameof(ItemOneViewFacade), "Assets/Scripts/MVVM/Tests/Prefabs/Polimorphyc/ItemOne.prefab" },
			{ nameof(ItemTwoViewFacade), "Assets/Scripts/MVVM/Tests/Prefabs/Polimorphyc/ItemTwo.prefab" },
			{ nameof(ItemThreeViewFacade), "Assets/Scripts/MVVM/Tests/Prefabs/Polimorphyc/ItemThree.prefab" },
			{ nameof(ItemsHolderViewFacade), "Assets/Scripts/MVVM/Tests/Prefabs/Polimorphyc/PolimorphycViewsholder.prefab" }
		};
		#endregion

		#region Interface Implementations
		public async UniTask<T> Get<T>(string key) where T : ViewFacade
		{
			if (!_pathMap.TryGetValue(key, out var path)) throw new KeyNotResolvedException(typeof(T), key);

			var go = InstantiateFromAssets(path);
			var facade = go.GetComponentInChildren<T>();

			return facade;
		}

		public async UniTask<ViewFacade> Get(string key)
		{
			if (!_pathMap.TryGetValue(key, out var path)) throw new KeyNotResolvedException(typeof(ViewFacade), key);

			var go = InstantiateFromAssets(path);
			var facade = go.GetComponentInChildren<ViewFacade>();

			return facade;
		}

		public void Release(ViewFacade viewFacade)
		{
			Object.Destroy(viewFacade.gameObject);
		}
		#endregion

		#region Private Members
		private GameObject InstantiateFromAssets(string path, Transform parent = null)
		{
			var goPrototype = AssetDatabase.LoadAssetAtPath<GameObject>(path);
			var go = Object.Instantiate(goPrototype, parent);

			return go;
		}
		#endregion
	}
}