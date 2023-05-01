using System;
using System.Collections.Generic;

public class ReactiveListRemoveRangeArgs< TValue > : EventArgs
{
	#region Public Fields
	public int Index;
	public int Count;
	public IEnumerable< TValue > removedValues;
	#endregion

	#region Constructors
	public ReactiveListRemoveRangeArgs()
	{
		removedValues = new List< TValue >();
	}

	public ReactiveListRemoveRangeArgs( int index, int count, IEnumerable< TValue > removed )
	{
		Index = index;
		Count = count;
		removedValues = removed;
	}
	#endregion
}
