using System;

namespace DM.Pooling
{
	public class PoolKeyNotResolvedException : PoolException
	{
		#region Constants
		private const string _message = "Can't resolve object of type: {0} with key: {1}.";
		#endregion

		#region Constructors
		public PoolKeyNotResolvedException(Type requestedType, string key, Exception inner) : base(string.Format(_message, requestedType, key), inner)
		{
		}
		#endregion
	}
}