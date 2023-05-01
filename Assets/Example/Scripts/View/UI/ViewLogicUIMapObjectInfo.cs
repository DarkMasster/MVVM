using Cysharp.Threading.Tasks;
using DM.MVVM.View;
using DM.ReactiveTypes;
using UnityEngine;

namespace DM.Example.Views.UI
{
	public class ViewLogicUIMapObjectInfo : ViewLogic<ViewFacadeUIMapObjectInfo, IViewModelMapObject>
	{
		#region Private Members
		private void HandleOnPositionChanged(object sender, GenericEventArg<Vector2Int> e)
		{
			ViewFacade.TextInfo.text = $"Pos = {e.Value}";
		}
		#endregion

		#region Overrides
		protected override async UniTask AssembleSubViewLogics()
		{
		}

		protected override async UniTask InitializeInternal()
		{
			ViewFacade.TextTitle.text = $"Type = {ViewModel.TypeName}";
			ViewFacade.TextInfo.text = $"Pos = {ViewModel.Position}";

			SubscribeAggregator.ListenEvent(ViewModel.Position, HandleOnPositionChanged);
		}

		protected override void DeInitializeInternal()
		{
		}
		#endregion
	}
}