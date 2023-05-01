using Cysharp.Threading.Tasks;
using DM.MVVM.View;
using DM.ReactiveTypes;
using UnityEngine;

namespace DM.Example.Views
{
	public class ViewLogicMapObject : ViewLogic<ViewFacadeMapObject, IViewModelMapObject>
	{
		#region Private Members
		private void HandleOnPositionChanged(object sender, GenericEventArg<Vector2Int> e)
		{
			ViewFacade.transform.position = new Vector3(e.Value.x, 0, e.Value.y);
		}
		#endregion

		#region Overrides
		protected override async UniTask AssembleSubViewLogics()
		{
		}

		protected override async UniTask InitializeInternal()
		{
			ViewFacade.transform.position = new Vector3(ViewModel.Position.Value.x, 0, ViewModel.Position.Value.y);
			SubscribeAggregator.ListenEvent(ViewModel.Position, HandleOnPositionChanged, true);
		}

		protected override void DeInitializeInternal()
		{
		}
		#endregion
	}
}