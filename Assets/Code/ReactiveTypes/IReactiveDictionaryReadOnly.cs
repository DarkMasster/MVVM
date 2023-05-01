using System;
using System.Collections.Generic;

namespace DM.ReactiveTypes
{
	public interface IReactiveDictionaryReadOnly< TKey, TValue > : IEnumerable< KeyValuePair< TKey, TValue > >
	{
		#region Properties
		TValue this[ TKey key ] { get; }
		#endregion

		#region Public Members
		event EventHandler< GenericPairEventArgs< TKey, TValue > > OnAddItem;
		event EventHandler< GenericEventArg< IDictionary< TKey, TValue > > > OnAddRange;
		event EventHandler< GenericEventArg< IDictionary< TKey, TValue > > > OnClear;
		event EventHandler< GenericPairEventArgs< TKey, TValue > > OnElementChange;
		event EventHandler< GenericPairEventArgs< TKey, TValue > > OnRemoveItem;

		int Count { get; }
		bool Contains( KeyValuePair< TKey, TValue > item );
		bool TryGetValue( TKey key, out TValue value );
		TValue GetSafe( TKey key );

		void SubscribeOnAddItem( TKey key, ReactiveDictionary< TKey, TValue >.ItemEventHandler handler );
		void UnsubscribeOnAddItem( TKey key, ReactiveDictionary< TKey, TValue >.ItemEventHandler handler );
		void SubscribeOnRemoveItem( TKey key, ReactiveDictionary< TKey, TValue >.ItemEventHandler handler );
		void UnsubscribeOnRemoveItem( TKey key, ReactiveDictionary< TKey, TValue >.ItemEventHandler handler );
		void SubscribeOnChangeItem( TKey key, ReactiveDictionary< TKey, TValue >.ItemEventHandler handler );
		void UnsubscribeOnChangeItem( TKey key, ReactiveDictionary< TKey, TValue >.ItemEventHandler handler );
		#endregion
	}
}