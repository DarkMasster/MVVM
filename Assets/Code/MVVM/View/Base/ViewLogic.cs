using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace DM.MVVM.View
{
	public abstract class ViewLogic<TViewFacade, TViewModel> : BaseViewLogic<TViewFacade, TViewModel>
		where TViewFacade : ViewFacade where TViewModel : IViewModel
	{
		#region Private Fields
		private readonly Dictionary<IViewModel, IViewLogic> _subViewLogics = new();
		#endregion
		
		#region Protected Members
		protected IViewLogic GetSubViewLogic(IViewModel viewModel) => _subViewLogics.TryGetValue(viewModel, out var viewLogic) ? viewLogic : default;

		protected T GetSubViewLogic<T>(IViewModel viewModel) where T : IViewLogic
		{
			var viewLogic = GetSubViewLogic(viewModel);

			if (viewLogic is T concreteViewLogic)
				return concreteViewLogic;

			return default;
		}

		protected void RegisterSubViewLogic(IViewModel viewModel, IViewLogic viewLogic)
		{
			if (_subViewLogics.ContainsKey(viewModel)) return;
			_subViewLogics.Add(viewModel, viewLogic);
		}

		protected async UniTask RegisterSubViewLogicWithInitialization
			(IViewModel viewModel, IViewLogic viewLogic)
		{
			if (_subViewLogics.ContainsKey(viewModel)) return;
			_subViewLogics.Add(viewModel, viewLogic);
			await viewLogic.Initialize();
		}

		protected void UnregisterSubViewLogic(IViewModel viewModel, bool deInitialize = true)
		{
			if (!_subViewLogics.TryGetValue(viewModel, out var viewLogic)) return;
			_subViewLogics.Remove(viewModel);
			viewLogic.DeInitialize();
		}

		protected abstract UniTask AssembleSubViewLogics();

		protected abstract UniTask InitializeInternal();
		protected abstract void DeInitializeInternal();
		#endregion

		#region Private Members
		private void DeInitializeSubViewLogics()
		{
			foreach (var logic in _subViewLogics.Values) logic.DeInitialize();
		}

		private async UniTask InitializeSubViewLogics()
		{
			await UniTask.WhenAll(_subViewLogics.Values.Select(l => l.Initialize()));
		}
		#endregion

		#region Overrides
		public sealed override void DeInitialize()
		{
			SubscribeAggregator.Unsubscribe();
			DeInitializeInternal();
			DeInitializeSubViewLogics();
			ViewFacadeFactory?.Release(ViewFacade);
		}

		public sealed override async UniTask Initialize()
		{
			await AssembleSubViewLogics();
			await InitializeSubViewLogics();
			await InitializeInternal();
		}
		#endregion
	}
}