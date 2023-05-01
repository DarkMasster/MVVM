using System;
using DM.MVVM.View;

namespace DM.Example.Factories
{
	public class ViewLogicFactory : IViewLogicFactory
	{
		#region Private Fields
		private readonly IViewFacadeFactory _viewFacadeFactory;
		#endregion

		#region Constructors
		public ViewLogicFactory(IViewFacadeFactory viewFacadeFactory) => _viewFacadeFactory = viewFacadeFactory;
		#endregion

		#region Interface Implementations
		public TViewLogic Create<TViewLogic>(Type viewLogicType, IViewModel viewModel, ViewFacade viewFacade) where TViewLogic : IViewLogic
		{
			try
			{
				var viewLogic = (TViewLogic) Activator.CreateInstance(viewLogicType);
				var context = new ViewLogicCreationContext(viewFacade, viewModel, this, _viewFacadeFactory);
				viewLogic.Construct(context);
				return viewLogic;
			}
			catch (Exception exception)
			{
				throw new IncompatibleParametersException(viewLogicType, viewFacade.GetType(), viewModel.GetType(), exception);
			}
		}


		public TViewLogic Create<TViewLogic>
			(IViewModel viewModel, ViewFacade viewFacade) where TViewLogic : IViewLogic =>
			Create<TViewLogic>(typeof(TViewLogic), viewModel, viewFacade);
		#endregion
	}
}