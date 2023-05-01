using System;

namespace DM.MVVM.View
{
	public abstract class ViewLogicFactoryException : AggregateException
	{
		#region Constructors
		protected ViewLogicFactoryException(string message, Exception inner)
		{
		}
		#endregion
	}

	public class IncompatibleParametersException : ViewLogicFactoryException
	{
		#region Constants
		private const string _message = "Parameters with types: {0}, {1}, {2} are incompatible.";
		#endregion

		#region Constructors
		public IncompatibleParametersException(Type logicType, Type facadeType, Type viewModelType, Exception inner)
			: base(string.Format(_message, logicType, facadeType, viewModelType), inner)
		{
		}
		#endregion
	}

	public class FacadeNotFoundException : ViewFacadeFactoryException
	{
		#region Constants
		private const string _message = "Can't find ViewFacade with type: {0} and key: {1}.";
		#endregion

		#region Constructors
		public FacadeNotFoundException(Type facadeType, string key, Exception inner) : base(string.Format(_message, facadeType, key), inner)
		{
		}
		#endregion
	}
}