using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DM.ReactiveTypes;
using MVVM.Tests.Polymorfic.View;
using UnityEditor;
using UnityEngine;

public class ItemsHolderViewModel : IItemsHolderViewModel
{
	#region Properties
	public IReadOnlyCollection<IBaseItemViewModel> Items => _itemsViewModels;
	#endregion

	#region Private Fields
	private readonly List<IBaseItemViewModel> _itemsViewModels;

	private readonly ItemsHolderModel _itemsHolder;
	#endregion

	#region Constructors
	public ItemsHolderViewModel(ItemsHolderModel itemsHolder)
	{
		_itemsHolder = itemsHolder;
		_itemsViewModels = _itemsHolder.Items.Select(CreateItemViewModel).ToList();
	}
	#endregion

	#region Private Members
	private static IBaseItemViewModel CreateItemViewModel(BaseItem item)
	{
		return item switch
		{
			ItemOne itemOne => new ItemOneViewModel(itemOne),
			ItemThree itemThree => new ItemThreeViewModel(itemThree),
			ItemTwo itemTwo => new ItemTwoViewModel(itemTwo),
			_ => default
		};
	}
	#endregion
}

public class ItemOneViewModel : IItemOneViewModel
{
	#region Properties
	public string Value => "42";
	#endregion

	#region Private Fields
	private readonly ItemOne _itemOne;
	#endregion

	#region Constructors
	public ItemOneViewModel(ItemOne itemOne)
	{
		_itemOne = itemOne;
		_itemOne.Greeting.OnValueChanged += HandleGreetingTestChanged;
	}
	#endregion

	#region Private Members
	private void HandleGreetingTestChanged(object sender, GenericEventArg<string> e)
	{
	}
	#endregion
}

public class ItemTwoViewModel : IItemTwoViewModel
{
	#region Properties
	public string Value => "kek";
	public string SpritePath => "Assets/UIContent/Images/BGs/lordTG_art_4096х2048.png";
	#endregion

	#region Private Fields
	private readonly ItemTwo _itemTwo;
	#endregion

	#region Constructors
	public ItemTwoViewModel(ItemTwo item) => _itemTwo = item;
	#endregion
}

public class ItemThreeViewModel : IItemThreeViewModel
{
	#region Properties
	public string Value => "Hello!";
	public IReactivePropertyReadonly<float> Progress => _progress;
	#endregion

	#region Private Fields
	private readonly ItemThree _itemThree;

	private readonly ReactiveProperty<float> _progress = new();
	#endregion

	#region Constructors
	public ItemThreeViewModel(ItemThree itemThree)
	{
		_itemThree = itemThree;
		MutateProgress();
	}
	#endregion

	#region Private Members
	private async void MutateProgress()
	{
		while (true)
		{
			_progress.Value = Random.Range(0f, 1f);
			await UniTask.Delay(500);
		}
	}
	#endregion
}