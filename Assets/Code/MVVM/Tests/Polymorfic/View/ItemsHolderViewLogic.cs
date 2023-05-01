using System;
using System.Collections.Generic;
using DM.MVVM.View;
using Cysharp.Threading.Tasks;

namespace MVVM.Tests.Polymorfic.View
{
	public interface IItemsHolderViewModel : IViewModel
	{
		#region Properties
		IReadOnlyCollection<IBaseItemViewModel> Items { get; }
		#endregion
	}

	public class ItemsHolderViewLogic : ViewLogic<ItemsHolderViewFacade, IItemsHolderViewModel>
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

		#region Overrides
		protected override async UniTask AssembleSubViewLogics()
		{
			foreach (var item in ViewModel.Items)
			{
				var viewLogicType = _viewModelLogicMap[item.GetType()];
				var key = _viewLogicFacadeMap[viewLogicType].Name;
				var itemViewFacade = await ViewFacadeFactory.Get(key);

				var itemViewLogic = ViewLogicFactory.Create<IViewLogic>(viewLogicType, item, itemViewFacade);
				itemViewLogic.SetViewFacadeParent(ViewFacade.ItemsHolder);
				RegisterSubViewLogic(item, itemViewLogic);
			}
		}

		protected override async UniTask InitializeInternal()
		{
		}

		protected override void DeInitializeInternal()
		{
		}
		#endregion
	}
}