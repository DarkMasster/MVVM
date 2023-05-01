using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DM.MVVM.View
{
	public abstract class BaseViewLogic<TViewFacade, TViewModel> : IViewLogic
		where TViewFacade : ViewFacade where TViewModel : IViewModel
	{
		#region Properties
		protected SubscribeAggregator SubscribeAggregator { get; } = new();
		protected TViewModel ViewModel { get; set; }
		protected TViewFacade ViewFacade { get; set; }

		protected IViewLogicFactory ViewLogicFactory { get; private set; }

		protected IViewFacadeFactory ViewFacadeFactory { get; private set; }
		#endregion
		
		#region Interface Implementations
		public void Construct(ViewLogicCreationContext context)
		{
			ViewLogicFactory = context.ViewLogicFactory;
			ViewFacadeFactory = context.ViewFacadeFactory;
			ViewModel = (TViewModel) context.ViewModel;
			ViewFacade = (TViewFacade) context.ViewFacade;
		}

		public void SetViewFacadeParent(Transform parent)
		{
			ViewFacade.transform.SetParent(parent);
		}

		public void SetViewFacadeParent(Transform parent, Vector3 localPosition)
		{
			SetViewFacadeParent(parent);
			ViewFacade.transform.localPosition = localPosition;
		}

		public void SetViewFacadeParent(Transform parent, Vector3 localPosition, Vector3 localEulerAngles)
		{
			SetViewFacadeParent(parent);
			ViewFacade.transform.localPosition = localPosition;
			ViewFacade.transform.localEulerAngles = localEulerAngles;
		}

		public void SetViewFacadeParent(Transform parent, Vector3 localPosition, Vector3 localEulerAngles, Vector3 localScale)
		{
			SetViewFacadeParent(parent);
			ViewFacade.transform.localPosition = localPosition;
			ViewFacade.transform.localEulerAngles = localEulerAngles;
			ViewFacade.transform.localScale = localScale;
		}

		public abstract UniTask Initialize();
		public abstract void DeInitialize();
		#endregion
	}
}