using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DM.MVVM.View
{
	public class ViewLogicCreationContext
	{
		#region Properties
		public ViewFacade ViewFacade { get; }
		public IViewModel ViewModel { get; }

		public IViewLogicFactory ViewLogicFactory { get; }

		public IViewFacadeFactory ViewFacadeFactory { get; }
		#endregion

		#region Constructors
		public ViewLogicCreationContext
			(ViewFacade viewFacade, IViewModel viewModel, IViewLogicFactory viewLogicFactory, IViewFacadeFactory viewFacadeFactory)
		{
			ViewFacade = viewFacade;
			ViewModel = viewModel;
			ViewLogicFactory = viewLogicFactory;
			ViewFacadeFactory = viewFacadeFactory;
		}
		#endregion
	}

	public interface IViewLogic
	{
		#region Public Members
		public void Construct(ViewLogicCreationContext context);

		void SetViewFacadeParent(Transform parent);

		void SetViewFacadeParent(Transform parent, Vector3 localPosition);

		void SetViewFacadeParent(Transform parent, Vector3 localPosition, Vector3 localEulerAngles);
		void SetViewFacadeParent(Transform parent, Vector3 localPosition, Vector3 localEulerAngles, Vector3 localScale);

		UniTask Initialize();

		void DeInitialize();
		#endregion
	}
}