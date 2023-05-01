using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DM.MVVM.View;
using DM.ReactiveTypes;

namespace DM.Example.Views.UI
{
	public class ViewLogicUIMapInfo : ViewLogic<ViewFacadeUIMapInfo, IViewModelMap>
	{
		#region Private Members
		private async UniTask AddItemSubViewLogic(IViewModelMapObject viewModel, bool initialize = false)
		{
			var viewFacade = await ViewFacadeFactory.Get("ViewFacadeUIMapObjectInfo");
			var viewLogic = ViewLogicFactory.Create<ViewLogicUIMapObjectInfo>(viewModel, viewFacade);
			viewLogic.SetViewFacadeParent(ViewFacade.ContentRoot);

			if (initialize)
				await RegisterSubViewLogicWithInitialization(viewModel, viewLogic);
			else
				RegisterSubViewLogic(viewModel, viewLogic);
		}

		private async void HandleOnObjectAdd(object sender, GenericPairEventArgs<int, IViewModelMapObject> e)
		{
			await AddItemSubViewLogic(e.Value, true);
		}

		private void HandleOnObjectRemove(object sender, GenericPairEventArgs<int, IViewModelMapObject> e)
		{
			UnregisterSubViewLogic(e.Value);
		}

		private void HandleObjectsClear(object sender, GenericEventArg<IDictionary<int, IViewModelMapObject>> e)
		{
			foreach (var viewModelMapObject in e.Value.Values) UnregisterSubViewLogic(viewModelMapObject);
		}

		private void HandleOnButtonAddNewClicked()
		{
			ViewModel.RequestAddNewMapObject();
		}

		private void HandleOnButtonClearClicked()
		{
			ViewModel.RequestClear();
		}
		#endregion

		#region Overrides
		protected override async UniTask AssembleSubViewLogics()
		{
			foreach (var item in ViewModel.Objects) await AddItemSubViewLogic(item.Value);
		}

		protected override async UniTask InitializeInternal()
		{
			ViewFacade.Text.text = $"Map {ViewModel.Size}";
			SubscribeAggregator.ListenEventDictionaryAddItem(ViewModel.Objects, HandleOnObjectAdd);
			SubscribeAggregator.ListenEventDictionaryRemoveItem(ViewModel.Objects, HandleOnObjectRemove);
			SubscribeAggregator.ListenEventDictionaryClear(ViewModel.Objects, HandleObjectsClear);
			SubscribeAggregator.ListenEvent(ViewFacade.ButtonAddNew.onClick, HandleOnButtonAddNewClicked);
			SubscribeAggregator.ListenEvent(ViewFacade.ButtonClear.onClick, HandleOnButtonClearClicked);
		}

		protected override void DeInitializeInternal()
		{
		}
		#endregion
	}
}