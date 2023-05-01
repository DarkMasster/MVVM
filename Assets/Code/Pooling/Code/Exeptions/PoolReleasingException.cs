using System;

namespace DM.Pooling
{
	public class PoolReleasingException : PoolException
	{
		#region Constants
		private const string _message = "Key: {0}, was not presented in pool.";
		#endregion

		#region Constructors
		public PoolReleasingException(string key, Exception inner) : base(string.Format(_message, key), inner)
		{
		}
		#endregion
	}
}