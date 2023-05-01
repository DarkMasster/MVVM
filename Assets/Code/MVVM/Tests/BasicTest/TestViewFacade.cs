using DM.MVVM.View;
using UnityEngine;
using UnityEngine.UI;

public class TestViewFacade : ViewFacade
{
	#region Properties
	[field : SerializeField]
	public Button Button { get; private set; }

	[field : SerializeField]
	public Text Text { get; private set; }
	#endregion
}