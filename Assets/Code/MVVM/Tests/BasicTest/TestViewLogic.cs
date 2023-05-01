using DM.MVVM.View;
using Cysharp.Threading.Tasks;
using DM.ReactiveTypes;

public interface ITestViewModel : IViewModel
{
	#region Properties
	IReactivePropertyReadonly<int> Number { get; }
	#endregion

	#region Public Members
	void HandleButtonClick();
	#endregion
}

public class TestViewModel : ITestViewModel
{
	#region Properties
	public IReactivePropertyReadonly<int> Number => _model.Number;
	#endregion

	#region Private Fields
	private readonly TestModel _model;
	#endregion

	#region Constructors
	public TestViewModel(TestModel model) => _model = model;
	#endregion

	#region Interface Implementations
	public void HandleButtonClick()
	{
		_model.AddOne();
	}
	#endregion
}

public class TestViewLogic : ViewLogic<TestViewFacade, ITestViewModel>
{
	#region Private Members
	private void HandleButtonClick()
	{
		ViewModel.HandleButtonClick();
	}

	private void HandleValueChanged(object sender, GenericEventArg<int> e)
	{
		ViewFacade.Text.text = e.Value.ToString();
	}
	#endregion

	#region Overrides
	protected override async UniTask AssembleSubViewLogics()
	{
	}

	protected override async UniTask InitializeInternal()
	{
		SubscribeAggregator.ListenEvent(ViewFacade.Button.onClick, HandleButtonClick);
		SubscribeAggregator.ListenEvent(ViewModel.Number, HandleValueChanged, true);
	}

	protected override void DeInitializeInternal()
	{
	}
	#endregion
}