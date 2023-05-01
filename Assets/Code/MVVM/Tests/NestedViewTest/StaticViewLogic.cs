using DM.MVVM.View;
using Cysharp.Threading.Tasks;

public class StaticViewLogic : ViewLogic<StaticViewFacade, IStaticViewModel>
{
	#region Private Fields
	private ItemViewLogic _itemOneViewLogic;
	private ItemViewLogic _itemTwoViewLogic;
	private ItemViewLogic _itemThreeViewLogic;
	#endregion

	#region Overrides
	protected override async  UniTask AssembleSubViewLogics()
	{
		_itemOneViewLogic = ViewLogicFactory.Create<ItemViewLogic>(typeof(ItemViewLogic), ViewModel.ItemViewModelOne, ViewFacade.ItemOne);
		RegisterSubViewLogic(ViewModel.ItemViewModelOne, _itemOneViewLogic);
		_itemTwoViewLogic = ViewLogicFactory.Create<ItemViewLogic>(typeof(ItemViewLogic), ViewModel.ItemViewModelTwo, ViewFacade.ItemTwo);
		RegisterSubViewLogic(ViewModel.ItemViewModelTwo, _itemTwoViewLogic);

		_itemThreeViewLogic =
			ViewLogicFactory.Create<ItemViewLogic>(typeof(ItemViewLogic), ViewModel.ItemViewModelThree, ViewFacade.ItemThree);

		RegisterSubViewLogic(ViewModel.ItemViewModelThree, _itemThreeViewLogic);
	}

	protected override async UniTask InitializeInternal()
	{
	}

	protected override void DeInitializeInternal()
	{
	}
	#endregion
}