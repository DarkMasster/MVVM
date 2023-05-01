using DM.ReactiveTypes;

public class TestModel
{
	#region Properties
	public IReactivePropertyReadonly<int> Number => _number;
	#endregion

	#region Private Fields
	private readonly ReactiveProperty<int> _number = new();
	#endregion

	#region Public Members
	public void AddOne()
	{
		_number.Value++;
	}
	#endregion
}