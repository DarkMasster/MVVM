using DM.MVVM.View;
using DM.ReactiveTypes;
using UnityEngine;

namespace DM.Example.Views
{
	public interface IViewModelMap : IViewModel
	{
		#region Properties
		public IReactivePropertyReadonly<Vector2Int> Size { get; }
		public IReactiveDictionaryReadOnly<int, IViewModelMapObject> Objects { get; }
		public void RequestAddNewMapObject();
		public void RequestClear();
		#endregion
	}
}