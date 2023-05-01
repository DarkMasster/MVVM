using System;
using DM.Example.Data;
using DM.Example.Services.MapService;
using DM.ReactiveTypes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DM.Example.Views
{
	public class ViewModelMap : IViewModelMap, IDisposable
	{
		#region Properties
		public IReactivePropertyReadonly<Vector2Int> Size => _size;
		public IReactiveDictionaryReadOnly<int, IViewModelMapObject> Objects => _objects;
		#endregion

		#region Private Fields
		private readonly ReactiveProperty<Vector2Int> _size = new();
		private readonly ReactiveDictionary<int, IViewModelMapObject> _objects = new();
		private readonly IMapService _mapService;
		#endregion

		#region Constructors
		public ViewModelMap(IMapService mapService)
		{
			_mapService = mapService;
			_size.Value = _mapService.GetMapSize();

			foreach (var modelMapObject in _mapService.GetAllObjects()) _objects.Add(modelMapObject.Id, new ViewModelMapObject(modelMapObject.Id, _mapService));

			_mapService.OnObjectCreated += HandleOnObjectCreated;
			_mapService.OnObjectRemoved += HandleOnObjectRemove;
			_mapService.OnClear += HandleOnClear;
		}
		#endregion

		#region Interface Implementations
		public void RequestAddNewMapObject()
		{
			var size = _mapService.GetMapSize();
			var values = Enum.GetValues(typeof(EMapObjectType));
			var index = Random.Range(0, values.Length);

			_mapService.CreateObject(Random.Range(-size.x, size.x), Random.Range(-size.y, size.y), (EMapObjectType) values.GetValue(index));
		}

		public void RequestClear()
		{
			_mapService.Clear();
		}

		public void Dispose()
		{
			_mapService.OnObjectCreated -= HandleOnObjectCreated;
			_mapService.OnObjectRemoved -= HandleOnObjectRemove;
		}
		#endregion

		#region Private Members
		private void HandleOnClear()
		{
			_objects.Clear();
		}

		private void HandleOnObjectCreated(int id, int x, int y, EMapObjectType type)
		{
			_objects.Add(id, new ViewModelMapObject(id, _mapService));
		}

		private void HandleOnObjectRemove(int id)
		{
			_objects.Remove(id);
		}
		#endregion
	}
}