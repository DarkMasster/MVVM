using UnityEditor;
using UnityEngine;

namespace DM.TestUtils
{
	public static class EditorTestUtils
	{
		#region Public Members
		public static GameObject GetFromAssets(string path)
		{
#if UNITY_EDITOR
			var goPrototype = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			return goPrototype;
#else
			return null;
#endif
		}

		public static T InstantiateFromAssets<T>(string path, Transform parent = null) where T : Component
		{
			var go = InstantiateFromAssets(path);
			var component = go.GetComponentInChildren<T>();

			return component;
		}

		public static GameObject InstantiateFromAssets(string path, Transform parent = null)
		{
			var goPrototype = GetFromAssets(path);
			var go = Object.Instantiate(goPrototype, parent);

			return go;
		}
		#endregion
	}
}