using DM.MVVM.View;
using DM.ReactiveTypes;
using UnityEngine;

namespace DM.Example.Views
{
	public interface IViewModelMapObject : IViewModel
	{
		#region Properties
		IReactivePropertyReadonly<Vector2Int> Position { get; }
		IReactivePropertyReadonly<int> Id { get; }
		string TypeName{ get; }
		#endregion
	}
}