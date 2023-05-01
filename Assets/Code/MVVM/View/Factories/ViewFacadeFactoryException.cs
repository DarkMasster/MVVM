using System;

namespace DM.MVVM.View
{
	public abstract class ViewFacadeFactoryException : AggregateException
	{
		#region Constructors
		protected ViewFacadeFactoryException(string message, Exception inner) : base(message, inner)
		{
		}
		#endregion
	}

	public class KeyNotResolvedException : ViewFacadeFactoryException
	{
		#region Constants
		private const string _message = "Can't resolve facade of type: {0} with key: {1}.";
		#endregion

		#region Constructors
		public KeyNotResolvedException(Type facadeType, string key) : base(string.Format(_message, facadeType, key), new Exception())
		{
		}
		#endregion
	}
}