using DM.MVVM.View;
using UnityEngine;

public class DynamicViewFacade : ViewFacade
{
	#region Properties
	[field : SerializeField]
	public GameObject ContentHolder { get; private set; }
	#endregion
}