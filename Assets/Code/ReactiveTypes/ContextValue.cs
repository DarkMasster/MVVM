namespace DM.ReactiveTypes
{
	public class ContextValue< TContext, TValue >
	{
		#region Properties
		public TContext Context { get; protected set; }
		public TValue Value { get; protected set; }
		#endregion

		#region Constructors
		public ContextValue( TContext context, TValue value )
		{
			Context = context;
			Value = value;
		}
		#endregion
	}
}