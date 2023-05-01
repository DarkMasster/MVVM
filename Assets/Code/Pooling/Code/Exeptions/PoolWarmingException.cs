using System;

namespace DM.Pooling
{
	public class PoolWarmingException : PoolException
	{
		#region Constants
		private const string _message = "Exception while worming key: {0}.";
		#endregion

		#region Constructors
		public PoolWarmingException(string key, Exception inner) : base(string.Format(_message, key), inner)
		{
		}
		#endregion
	}
}