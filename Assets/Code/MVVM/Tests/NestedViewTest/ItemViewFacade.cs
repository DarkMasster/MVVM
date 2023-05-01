using DM.MVVM.View;
using UnityEngine;
using UnityEngine.UI;

public class ItemViewFacade : ViewFacade
{
	#region Properties
	[field : SerializeField]
	public Image Image { get; private set; }

	[field : SerializeField]
	public Text Text { get; private set; }
	#endregion
}