using DM.MVVM.View;
using TMPro;
using UnityEngine;

namespace DM.Example.Views.UI
{
	public class ViewFacadeUIMapObjectInfo : ViewFacade
	{
		#region UnitySerialized
		[field : SerializeField] public TextMeshProUGUI TextTitle;
		[field : SerializeField] public TextMeshProUGUI TextInfo;
		#endregion
	}
}