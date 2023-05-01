using System;
using System.Collections.Generic;
using DM.Example.Data;
using UnityEngine;

namespace DM.Example.Services.MapService
{
	public interface IMapService
	{
		#region Events
		event Action<int, int, int, EMapObjectType> OnObjectCreated;
		event Action<int, int, int> OnObjectPositionChanged;
		event Action<int> OnObjectRemoved;
		event Action OnClear;
		#endregion

		#region Public Members
		bool IsObjectExist(int id);
		int CreateObject(int x, int y, EMapObjectType objectType);
		void RemoveObject(int id);
		void SetObjectPosition(int id, int x, int y);
		Vector2Int GetObjectPosition(int id);
		EMapObjectType GetObjectType(int id);
		IEnumerable<MapObjectData> GetAllObjects();
		Vector2Int GetMapSize();
		void Clear();
		#endregion
	}
}