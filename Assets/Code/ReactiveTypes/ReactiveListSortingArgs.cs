using System;

public class ReactiveListSortingArgs< TValue > : EventArgs
{
	#region Public Fields
	public int OldIndex;
	public int NewIndex;
	public TValue Value;
	#endregion

	#region Constructors
	public ReactiveListSortingArgs()
	{
	}

	public ReactiveListSortingArgs( int oldIndex, int newIndex, TValue value )
	{
		OldIndex = oldIndex;
		NewIndex = newIndex;
		Value = value;
	}
	#endregion
}
