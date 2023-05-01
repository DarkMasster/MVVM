using System;
using System.Collections.Generic;
using System.Linq;
using DM.Example.Data;
using UnityEngine;

namespace DM.Example.Services.MapService
{
	public class MapService : IMapService
	{
		#region Events
		public event Action<int, int, int, EMapObjectType> OnObjectCreated;
		public event Action<int, int, int> OnObjectPositionChanged;
		public event Action<int, EMapObjectType> OnObjectTypeChanged;
		public event Action<int> OnObjectRemoved;
		public event Action OnClear;
		#endregion

		#region Private Fields
		private readonly MapData _mapData;
		#endregion

		#region Constructors
		public MapService() => _mapData = new MapData
		{
			SizeX = 10,
			SizeY = 10
		};
		#endregion

		#region Interface Implementations
		public bool IsObjectExist(int id)
		{
			return _mapData.Objects.FirstOrDefault(item => item.Id == id) != null;
		}

		public int CreateObject(int x, int y, EMapObjectType objectType)
		{
			var newObject = new MapObjectData
			{
				X = x,
				Y = y,
				Type = objectType
			};

			newObject.Id = newObject.GetHashCode();

			_mapData.Objects.Add(newObject);
			OnObjectCreated?.Invoke(newObject.Id, x, y, objectType);

			return newObject.Id;
		}

		public void RemoveObject(int id)
		{
			var mapObjectData = GetObject(id);
			_mapData.Objects.Remove(mapObjectData);
			OnObjectRemoved?.Invoke(id);
		}

		public void SetObjectPosition(int id, int x, int y)
		{
			var mapObjectData = GetObject(id);
			mapObjectData.X = x;
			mapObjectData.Y = x;
			OnObjectPositionChanged?.Invoke(id, x, y);
		}

		public Vector2Int GetObjectPosition(int id)
		{
			var mapObjectData = GetObject(id);

			return new Vector2Int(mapObjectData.X, mapObjectData.Y);
		}

		public EMapObjectType GetObjectType(int id)
		{
			var mapObjectData = GetObject(id);

			return mapObjectData.Type;
		}

		public IEnumerable<MapObjectData> GetAllObjects() => _mapData.Objects;
		public Vector2Int GetMapSize() => new(_mapData.SizeX, _mapData.SizeY);

		public void Clear()
		{
			_mapData.Objects.Clear();
			OnClear?.Invoke();
		}
		#endregion

		#region Private Members
		private MapObjectData GetObject(int id) => _mapData.Objects.First(item => item.Id == id);
		#endregion
	}
}