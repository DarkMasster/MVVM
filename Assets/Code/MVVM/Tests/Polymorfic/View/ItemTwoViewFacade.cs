using DM.MVVM.View;
using UnityEngine;
using UnityEngine.UI;

public class ItemTwoViewFacade : ViewFacade
{
	#region Properties
	[field : SerializeField]
	public Button Button { get; private set; }

	[field : SerializeField]
	public Text Text { get; private set; }

	[field : SerializeField]
	public Image Image { get; private set; }
	#endregion
}