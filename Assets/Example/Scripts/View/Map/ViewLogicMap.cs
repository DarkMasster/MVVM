using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DM.MVVM.View;
using DM.ReactiveTypes;
using UnityEngine;

namespace DM.Example.Views
{
	public class ViewLogicMap : ViewLogic<ViewFacadeMap, IViewModelMap>
	{
		#region Constants
		private static readonly Dictionary<string, string> _viewFacadesMap = new()
		{
			{ "Simple", "ViewFacadeMapObject" },
			{ "TypeA", "ViewFacadeMapObjectA" },
			{ "TypeB", "ViewFacadeMapObjectB" },
			{ "TypeC", "ViewFacadeMapObjectC" }
		};
		#endregion

		#region Private Members
		private async UniTask AddItemSubViewLogic(IViewModelMapObject viewModel, bool initialize = false)
		{
			var viewFacade = await ViewFacadeFactory.Get(_viewFacadesMap[viewModel.TypeName]);
			var viewLogic = ViewLogicFactory.Create<ViewLogicMapObject>(viewModel, viewFacade);
			viewLogic.SetViewFacadeParent(ViewFacade.transform);

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
		#endregion

		#region Overrides
		protected override async UniTask AssembleSubViewLogics()
		{
			foreach (var item in ViewModel.Objects) await AddItemSubViewLogic(item.Value);
		}

		protected override async UniTask InitializeInternal()
		{
			var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
			ground.transform.SetParent(ViewFacade.transform);
			ground.transform.localScale = new Vector3(ViewModel.Size.Value.x, 1, ViewModel.Size.Value.y);

			SubscribeAggregator.ListenEventDictionaryAddItem(ViewModel.Objects, HandleOnObjectAdd);
			SubscribeAggregator.ListenEventDictionaryRemoveItem(ViewModel.Objects, HandleOnObjectRemove);
			SubscribeAggregator.ListenEventDictionaryClear(ViewModel.Objects, HandleObjectsClear);
		}

		protected override void DeInitializeInternal()
		{
		}
		#endregion
	}
}