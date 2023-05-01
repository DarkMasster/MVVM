using DM.MVVM.View;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public interface IItemViewModel : IViewModel
{
	#region Properties
	public int Value { get; }
	public Sprite Sprite { get; }
	#endregion
}

public class ItemViewModel : IItemViewModel
{
	#region Properties
	public int Value => _item.Value;
	public Sprite Sprite => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UIContent/Images/BGs/lordTG_art_4096х2048.png");
	#endregion

	#region Private Fields
	private readonly TestItem _item;
	#endregion

	#region Constructors
	public ItemViewModel(TestItem item) => _item = item;
	#endregion
}

public class ItemViewLogic : ViewLogic<ItemViewFacade, IItemViewModel>
{
	
	#region Overrides
	protected override async UniTask AssembleSubViewLogics()
	{
	}

	protected override async UniTask InitializeInternal()
	{
		ViewFacade.Image.sprite = ViewModel.Sprite;
		ViewFacade.Text.text = ViewModel.Value.ToString();
	}

	protected override void DeInitializeInternal()
	{
	}
	#endregion
}