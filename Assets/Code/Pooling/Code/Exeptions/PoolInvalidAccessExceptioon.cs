using System;

namespace DM.Pooling
{
	public class PoolInvalidAccessExceptioon : PoolException
	{
		#region Constants
		private const string _message = "Pool is warming. Pool acces is forbiden.";
		#endregion

		#region Constructors
		public PoolInvalidAccessExceptioon() : base(_message, new Exception())
		{
		}
		#endregion
	}
}