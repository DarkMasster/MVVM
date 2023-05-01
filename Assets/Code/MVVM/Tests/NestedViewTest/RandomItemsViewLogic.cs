using System;
using System.Collections.Generic;
using DM;
using DM.MVVM.View;
using Cysharp.Threading.Tasks;
using DM.ReactiveTypes;
using MVVM.Tests.Polymorfic.View;

public interface IRandomItemsViewModel : IViewModel
{
	#region Properties
	public IReactiveListReadOnly<IItemViewModel> Items { get; }
	#endregion
}

public class RandomItemsViewModel : IRandomItemsViewModel
{
	#region Properties
	public IReactiveListReadOnly<IItemViewModel> Items => _nestedViewModels;
	#endregion

	#region Private Fields
	private readonly ReactiveList<IItemViewModel> _nestedViewModels = new();

	private readonly SubscribeAggregator _subscribeAggregator = new();
	private readonly RandomItemsModel _randomItemsModel;
	#endregion

	#region Constructors
	public RandomItemsViewModel(RandomItemsModel model)
	{
		_randomItemsModel = model;
		_subscribeAggregator.ListenEventListAddElement((ReactiveList<TestItem>) _randomItemsModel.Items, HandleAddElement);
		_subscribeAggregator.ListenEventListRemoveItem((ReactiveList<TestItem>) _randomItemsModel.Items, HandleRemoveElement);
		PopulateNestedViewModels();
	}
	#endregion

	#region Private Members
	private void PopulateNestedViewModels()
	{
		foreach (var item in _randomItemsModel.Items) AddItem(item);
	}

	private void HandleRemoveElement(object sender, GenericPairEventArgs<int, TestItem> e)
	{
		_nestedViewModels.RemoveAt(e.Key);
	}

	private void HandleAddElement(object sender, GenericPairEventArgs<int, TestItem> e)
	{
		AddItem(e.Value);
	}

	private void AddItem(TestItem item)
	{
		var viewModel = new ItemViewModel(item);
		_nestedViewModels.Add(viewModel);
	}
	#endregion
}

public class RandomItemsViewLogic : ViewLogic<DynamicViewFacade, IRandomItemsViewModel>
{
	#region Private Fields
	private readonly Dictionary<Type, Type> _viewModelLogicMap = new()
	{
		{ typeof(ItemsHolderViewModel), typeof(ItemsHolderViewLogic) },
		{ typeof(ItemOneViewModel), typeof(ItemOneViewLogic) },
		{ typeof(ItemTwoViewModel), typeof(ItemTwoViewLogic) },
		{ typeof(ItemThreeViewModel), typeof(ItemThreeViewLogic) },
		{ typeof(TestViewModel), typeof(TestViewLogic) },
		{ typeof(ItemViewModel), typeof(ItemViewLogic) },
		{ typeof(StaticViewModel), typeof(StaticViewLogic) },
		{ typeof(RandomItemsViewModel), typeof(RandomItemsViewLogic) }
	};

	private readonly Dictionary<Type, Type> _viewLogicFacadeMap = new()
	{
		{ typeof(ItemsHolderViewLogic), typeof(ItemsHolderViewFacade) },
		{ typeof(ItemOneViewLogic), typeof(ItemOneViewFacade) },
		{ typeof(ItemTwoViewLogic), typeof(ItemTwoViewFacade) },
		{ typeof(ItemThreeViewLogic), typeof(ItemThreeViewFacade) },
		{ typeof(TestViewLogic), typeof(TestViewFacade) },
		{ typeof(ItemViewLogic), typeof(ItemViewFacade) },
		{ typeof(StaticViewLogic), typeof(StaticViewFacade) },
		{ typeof(RandomItemsViewLogic), typeof(DynamicViewFacade) }
	};
	#endregion
	
	#region Private Members
	private void HandleElementRemoved(object sender, GenericPairEventArgs<int, IItemViewModel> e)
	{
		UnregisterSubViewLogic(e.Value);
	}

	private void HandleElementAdded(object sender, GenericPairEventArgs<int, IItemViewModel> e)
	{
		AddItemSubViewLogic(e.Value, true).Forget();
	}

	private async UniTask AddItemSubViewLogic(IItemViewModel viewModel, bool initialize = false)
	{
		var viewLogicType = _viewModelLogicMap[viewModel.GetType()];
		var key = _viewLogicFacadeMap[_viewModelLogicMap[viewModel.GetType()]].Name;
		var viewFacade = await ViewFacadeFactory.Get(key);
		var viewLogic = ViewLogicFactory.Create<ItemViewLogic>(viewLogicType, viewModel, viewFacade);
		viewLogic.SetViewFacadeParent(ViewFacade.ContentHolder.transform);

		if (initialize)
			await RegisterSubViewLogicWithInitialization(viewModel, viewLogic);
		else
			RegisterSubViewLogic(viewModel, viewLogic);
	}
	#endregion

	#region Overrides
	protected override async UniTask AssembleSubViewLogics()
	{
		foreach (var item in ViewModel.Items) await AddItemSubViewLogic(item);
	}

	protected override async UniTask InitializeInternal()
	{
		SubscribeAggregator.ListenEventListAddElement((ReactiveList<IItemViewModel>) ViewModel.Items,
													  HandleElementAdded);

		SubscribeAggregator.ListenEventListRemoveItem((ReactiveList<IItemViewModel>) ViewModel.Items,
													  HandleElementRemoved);
	}

	protected override void DeInitializeInternal()
	{
	}
	#endregion
}