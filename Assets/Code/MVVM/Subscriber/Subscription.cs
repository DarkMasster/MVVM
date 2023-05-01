using System;

namespace DM
{
	public class Subscription
	{
		#region Public Fields
		public Delegate OriginalHandler;
		public Action UnsubscribeHandler;
		#endregion

		#region Constructors
		public Subscription(Delegate originalHandler, Action unsubscribeHandler)
		{
			OriginalHandler = originalHandler;
			UnsubscribeHandler = unsubscribeHandler;
		}
		#endregion
	}
}