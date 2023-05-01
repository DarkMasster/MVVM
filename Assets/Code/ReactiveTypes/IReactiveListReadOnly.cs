using System;
using System.Collections.Generic;

namespace DM.ReactiveTypes
{
	public interface IReactiveListReadOnly< T > : IEnumerable< T >
	{
		#region Properties
		int Count { get; }
		T this[ int index ] { get; }
		#endregion

		#region Public Members
		int IndexOf( T item );

		event EventHandler< GenericPairEventArgs< int, T > > OnAddItem;
		event EventHandler< GenericEventArg< IEnumerable< T > > > OnAddRange;
		event EventHandler< GenericEventArg< IEnumerable< T > > > OnClear;
		event EventHandler< GenericPairEventArgs< int, T > > OnElementChange;
		event EventHandler< GenericPairEventArgs< int, T > > OnRemoveItem;
		event EventHandler< ReactiveListSortingArgs< T > > OnSort;
		
		#endregion
	}
}