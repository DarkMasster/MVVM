using DM.MVVM.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DM.Example.Views.UI
{
	public class ViewFacadeUIMapInfo : ViewFacade
	{
		#region UnitySerialized
		[field : SerializeField] public TextMeshProUGUI Text;
		[field : SerializeField] public Transform ContentRoot;
		[field : SerializeField] public Button ButtonAddNew;
		[field : SerializeField] public Button ButtonClear;
		#endregion
	}
}