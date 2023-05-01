using DM.MVVM.View;
using UnityEngine;

namespace MVVM.Tests.Polymorfic.View
{
	public class ItemsHolderViewFacade : ViewFacade
	{
		#region Properties
		[field : SerializeField]
		public Transform ItemsHolder { get; private set; }
		#endregion
	}
}