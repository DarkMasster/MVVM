using DM.MVVM.View;
using Cysharp.Threading.Tasks;
using DM.ReactiveTypes;
using UnityEditor;
using UnityEngine;

namespace MVVM.Tests.Polymorfic.View
{
	public interface IBaseItemViewModel : IViewModel
	{
	}

	public interface IItemOneViewModel : IBaseItemViewModel
	{
		#region Properties
		string Value { get; }
		#endregion
	}

	public class ItemOneViewLogic : ViewLogic<ItemOneViewFacade, IItemOneViewModel>
	{
		
		#region Overrides
		protected override async UniTask AssembleSubViewLogics()
		{
		}

		protected override async UniTask InitializeInternal()
		{
			ViewFacade.Text.text = ViewModel.Value;
		}

		protected override void DeInitializeInternal()
		{
		}
		#endregion
	}

	public interface IItemTwoViewModel : IBaseItemViewModel
	{
		#region Properties
		string Value { get; }
		string SpritePath { get; }
		#endregion
	}

	public class ItemTwoViewLogic : ViewLogic<ItemTwoViewFacade, IItemTwoViewModel>
	{

		#region Overrides
		protected override async UniTask AssembleSubViewLogics()
		{
		}

		protected override async UniTask InitializeInternal()
		{
			ViewFacade.Text.text = ViewModel.Value;
			ViewFacade.Image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(ViewModel.SpritePath);
		}

		protected override void DeInitializeInternal()
		{
		}
		#endregion
	}

	public interface IItemThreeViewModel : IBaseItemViewModel
	{
		#region Properties
		string Value { get; }
		IReactivePropertyReadonly<float> Progress { get; }
		#endregion
	}

	public class ItemThreeViewLogic : ViewLogic<ItemThreeViewFacade, IItemThreeViewModel>
	{

		#region Private Members
		private void HandleSliderValueUpdated(object sender, GenericEventArg<float> e)
		{
			ViewFacade.Slider.value = e.Value;
		}
		#endregion

		#region Overrides
		protected override async UniTask AssembleSubViewLogics()
		{
		}

		protected override async UniTask InitializeInternal()
		{
			ViewFacade.Text.text = ViewModel.Value;
			SubscribeAggregator.ListenEvent(ViewModel.Progress, HandleSliderValueUpdated);
		}

		protected override void DeInitializeInternal()
		{
		}
		#endregion
	}
}