using System;

namespace DM.Pooling
{
	public abstract class PoolException : AggregateException
	{
		#region Constructors
		protected PoolException(string message, Exception inner) : base(message, inner)
		{
		}
		#endregion
	}
}