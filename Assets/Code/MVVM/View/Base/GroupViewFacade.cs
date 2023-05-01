using System.Collections.Generic;
using UnityEngine;

namespace DM.MVVM.View
{
	[ExecuteAlways]
	public class GroupViewFacade : ViewFacade
	{
		#region UnitySerialized
		[SerializeField] private List<GameObject> ParticipantsGameObjects;
		#endregion

		#region Properties
		public IEnumerable<GameObject> Participants => ParticipantsGameObjects;
		#endregion

		#region UnityMembers
		private void OnEnable()
		{
			foreach (var participant in Participants) participant.SetActive(true);
		}

		private void OnDisable()
		{
			foreach (var participant in Participants) participant.SetActive(false);
		}
		#endregion
	}
}