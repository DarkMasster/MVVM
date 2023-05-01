using DM.MVVM.View;

public interface IStaticViewModel : IViewModel
{
	#region Properties
	ItemViewModel ItemViewModelOne { get; }
	ItemViewModel ItemViewModelTwo { get; }
	ItemViewModel ItemViewModelThree { get; }
	#endregion
}

public class StaticViewModel : IStaticViewModel
{
	#region Properties
	public ItemViewModel ItemViewModelOne { get; }
	public ItemViewModel ItemViewModelTwo { get; }
	public ItemViewModel ItemViewModelThree { get; }
	#endregion

	#region Private Fields
	private readonly StaticModel _staticModel;
	#endregion

	#region Constructors
	public StaticViewModel(StaticModel staticModel)
	{
		_staticModel = staticModel;
		ItemViewModelOne = new ItemViewModel(_staticModel.ItemOne);
		ItemViewModelTwo = new ItemViewModel(_staticModel.ItemTwo);
		ItemViewModelThree = new ItemViewModel(_staticModel.ItemThree);
	}
	#endregion
}