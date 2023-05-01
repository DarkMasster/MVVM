using System.Collections.Generic;

namespace DM.Pooling
{
	public class PoolInfo
	{
		#region Properties
		public List<KeyCapacityPair> AvailableObjectsStatistic { get; set; }
		public List<KeyCapacityPair> TrackedObjectsStatistic { get; set; }
		#endregion
	}
}