using System.Collections.Generic;
using DM.ReactiveTypes;

public class ItemsHolderModel
{
	#region Properties
	public IReadOnlyCollection<BaseItem> Items => _items;
	#endregion

	#region Private Fields
	private readonly List<BaseItem> _items = new()
	{
		new ItemOne(),
		new ItemTwo(),
		new ItemThree()
	};
	#endregion
}

public abstract class BaseItem
{
	#region Properties
	public IReactivePropertyReadonly<string> Greeting => greeting;
	#endregion

	#region Protected Fields
	protected readonly ReactiveProperty<string> greeting = new();
	#endregion

	#region Public Members
	public abstract void SayHello();
	#endregion
}

public class ItemOne : BaseItem
{
	#region Properties
	public int ItemOneValue => 42;
	#endregion

	#region Overrides
	public override void SayHello()
	{
		greeting.Value = "Hello from item one";
	}
	#endregion
}

public class ItemTwo : BaseItem
{
	#region Properties
	public int ItemTwoValue => 42;
	#endregion

	#region Overrides
	public override void SayHello()
	{
		greeting.Value = "Hello from item one";
	}
	#endregion
}

public class ItemThree : BaseItem
{
	#region Properties
	public int ItemThreeValue => 42;
	#endregion

	#region Overrides
	public override void SayHello()
	{
		greeting.Value = "Hello from item three";
	}
	#endregion
}