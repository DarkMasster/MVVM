using System;

namespace DM.MVVM.View
{
	public interface IViewLogicFactory
	{
		#region Public Members
		TViewLogic Create<TViewLogic>(Type viewLogicType, IViewModel viewModel, ViewFacade viewFacade) where TViewLogic : IViewLogic;

		TViewLogic Create<TViewLogic>(IViewModel viewModel, ViewFacade viewFacade) where TViewLogic : IViewLogic;
		#endregion
	}
}