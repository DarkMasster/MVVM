using System;
using DM.Example.Services.MapService;
using DM.ReactiveTypes;
using UnityEngine;

namespace DM.Example.Views
{
	public class ViewModelMapObject : IViewModelMapObject, IDisposable
	{
		#region Properties
		public IReactivePropertyReadonly<Vector2Int> Position => _position;
		public IReactivePropertyReadonly<int> Id => _id;
		public string TypeName { get; }
		#endregion

		#region Private Fields
		private readonly ReactiveProperty<Vector2Int> _position = new();
		private readonly ReactiveProperty<int> _id = new();
		private readonly IMapService _mapService;
		#endregion

		#region Constructors
		public ViewModelMapObject(int id, IMapService mapService)
		{
			_mapService = mapService;
			_mapService.OnObjectPositionChanged += HandleOnMapObjectPositionChanged;

			_id.Value = id;
			_position.Value = _mapService.GetObjectPosition(id);
			TypeName = _mapService.GetObjectType(id).ToString();
		}
		#endregion

		#region Interface Implementations
		public void Dispose()
		{
			_mapService.OnObjectPositionChanged -= HandleOnMapObjectPositionChanged;
		}
		#endregion

		#region Private Members
		private void HandleOnMapObjectPositionChanged(int id, int x, int y)
		{
			if (id != _id.Value) return;

			_position.Value = new Vector2Int(x, y);
		}
		#endregion
	}
}