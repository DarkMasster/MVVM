using System;
using System.Collections.Generic;
using DM.ReactiveTypes;
using UnityEngine.Events;

namespace DM
{
	public class SubscribeAggregator
	{
		#region Private Fields
		private readonly Dictionary<object, List<Subscription>> _subscriptions = new();
		#endregion

		#region Public Members
		public void Unsubscribe()
		{
			foreach (var containerInfo in _subscriptions)
			{
				foreach (var subscription in containerInfo.Value) subscription.UnsubscribeHandler();
			}

			_subscriptions.Clear();
		}

		public void ListenEvent<TType>
			(
			UnityEvent<TType> eventObject,
			UnityAction<TType> handler,
			bool invokeAfterSubscribe = false,
			TType invokeValue = default
			)
		{
			eventObject.AddListener(handler);
			Action unsubscriptionDelegate = () => eventObject.RemoveListener(handler);
			AddSubscription(eventObject, unsubscriptionDelegate, handler);
			if (invokeAfterSubscribe) handler(invokeValue);
		}

		public void ListenEvent<TType, TType2>
			(
			UnityEvent<TType, TType2> eventObject,
			UnityAction<TType, TType2> handler,
			bool invokeAfterSubscribe = false,
			TType invokeValue = default,
			TType2 invokeValue2 = default
			)
		{
			eventObject.AddListener(handler);
			Action unsubscriptionDelegate = () => eventObject.RemoveListener(handler);
			AddSubscription(eventObject, unsubscriptionDelegate, handler);
			if (invokeAfterSubscribe) handler(invokeValue, invokeValue2);
		}

		public void ListenEvent<TType, TType2, TType3>
			(
			UnityEvent<TType, TType2, TType3> eventObject,
			UnityAction<TType, TType2, TType3> handler,
			bool invokeAfterSubscribe = false,
			TType invokeValue = default,
			TType2 invokeValue2 = default,
			TType3 invokeValue3 = default
			)
		{
			eventObject.AddListener(handler);
			Action unsubscriptionDelegate = () => eventObject.RemoveListener(handler);
			AddSubscription(eventObject, unsubscriptionDelegate, handler);
			if (invokeAfterSubscribe) handler(invokeValue, invokeValue2, invokeValue3);
		}

		public void ListenEvent(UnityEvent eventObject, UnityAction handler, bool invokeAfterSubscribe = false)
		{
			eventObject.AddListener(handler);
			Action unsubscriptionDelegate = () => eventObject.RemoveListener(handler);
			AddSubscription(eventObject, unsubscriptionDelegate, handler);
			if (invokeAfterSubscribe) handler();
		}

		public void ListenEvent<TType>
			(
			IReactivePropertyReadonly<TType> property,
			EventHandler<GenericEventArg<TType>> handler,
			bool invokeAfterSubscribe = false
			)
		{
			property.OnValueChanged += handler;
			Action unsubscriptionDelegate = () => property.OnValueChanged -= handler;
			AddSubscription(property, unsubscriptionDelegate, handler);
			if (invokeAfterSubscribe) handler(property, new GenericEventArg<TType>(property.Value));
		}

		public void ListenEvent<TType>
			(
			IReactiveProperty<TType> property,
			EventHandler<GenericEventArg<TType>> handler,
			bool invokeAfterSubscribe = false
			)
		{
			property.OnValueChanged += handler;
			Action unsubscriptionDelegate = () => property.OnValueChanged -= handler;
			AddSubscription(property, unsubscriptionDelegate, handler);
			if (invokeAfterSubscribe) handler(property, new GenericEventArg<TType>(property.GetValue()));
		}

		public void ListenEvent<TType>
			(
			ReactiveProperty<TType> property,
			EventHandler<GenericEventArg<TType>> handler,
			bool invokeAfterSubscribe = false
			)
		{
			ListenEvent((IReactiveProperty<TType>) property, handler, invokeAfterSubscribe);
		}

		public void ListenEventExtended<TType>
			(
			ReactiveProperty<TType> property,
			EventHandler<PropertyEventArgs<TType>> handler,
			bool invokeAfterSubscribe = false
			)
		{
			property.OnValueChangedExtended += handler;
			Action unsubscriptionDelegate = () => property.OnValueChangedExtended -= handler;
			AddSubscription(property, unsubscriptionDelegate, handler);

			if (invokeAfterSubscribe)
				handler(property, new PropertyEventArgs<TType>(property.GetValue(), property.GetValue()));
		}

		public void ListenEventExtended<TType>
			(
			IReactiveProperty<TType> property,
			EventHandler<PropertyEventArgs<TType>> handler,
			bool invokeAfterSubscribe = false
			)
		{
			property.OnValueChangedExtended += handler;
			Action unsubscriptionDelegate = () => property.OnValueChangedExtended -= handler;
			AddSubscription(property, unsubscriptionDelegate, handler);

			if (invokeAfterSubscribe)
				handler(property, new PropertyEventArgs<TType>(property.GetValue(), property.GetValue()));
		}

		public void ListenEventExtended<TType>
			(
			IReactivePropertyReadonly<TType> property,
			EventHandler<PropertyEventArgs<TType>> handler,
			bool invokeAfterSubscribe = false
			)
		{
			property.OnValueChangedExtended += handler;
			Action unsubscriptionDelegate = () => property.OnValueChangedExtended -= handler;
			AddSubscription(property, unsubscriptionDelegate, handler);

			if (invokeAfterSubscribe)
				handler(property, new PropertyEventArgs<TType>((property as ReactiveProperty<TType>).GetValue(), (property as ReactiveProperty<TType>).GetValue()));
		}

		public void ListenEventListAddRange<TType>
			(
			ReactiveList<TType> list,
			EventHandler<GenericEventArg<IEnumerable<TType>>> handler,
			IEnumerable<TType> defaultArgs = null
			)
		{
			list.OnAddRange += handler;
			Action unsubscriptionDelegate = () => list.OnAddRange -= handler;
			AddSubscription(list, unsubscriptionDelegate, handler);
			if (defaultArgs != null) handler(list, new GenericEventArg<IEnumerable<TType>>(defaultArgs));
		}

		public void ListenEventListAddRange<TType>
			(
			IReactiveListReadOnly<TType> list,
			EventHandler<GenericEventArg<IEnumerable<TType>>> handler,
			IEnumerable<TType> defaultArgs = null
			)
		{
			list.OnAddRange += handler;
			Action unsubscriptionDelegate = () => list.OnAddRange -= handler;
			AddSubscription(list, unsubscriptionDelegate, handler);
			if (defaultArgs != null) handler(list, new GenericEventArg<IEnumerable<TType>>(defaultArgs));
		}

		public void ListenEventListAddElement<TType>
			(
			IReactiveListReadOnly<TType> list,
			EventHandler<GenericPairEventArgs<int, TType>> handler,
			bool invokeAfterSubscribe = false
			)
		{
			ListenEventListAddElement((ReactiveList<TType>) list, handler, invokeAfterSubscribe);
		}

		public void ListenEventListAddElement<TType>
			(
			ReactiveList<TType> list,
			EventHandler<GenericPairEventArgs<int, TType>> handler,
			bool invokeAfterSubscribe = false
			)
		{
			list.OnAddItem += handler;
			Action unsubscriptionDelegate = () => list.OnAddItem -= handler;
			AddSubscription(list, unsubscriptionDelegate, handler);

			if (invokeAfterSubscribe)
			{
				var _args = new GenericPairEventArgs<int, TType>();

				for (var i = 0; i < list.Count; i++)
				{
					_args.Key = i;
					_args.Value = list[i];
					handler(list, _args);
				}
			}
		}

		public void ListenEventListElementChange<TType>
			(ReactiveList<TType> list, EventHandler<GenericPairEventArgs<int, TType>> handler)
		{
			list.OnElementChange += handler;
			Action unsubscriptionDelegate = () => list.OnElementChange -= handler;
			AddSubscription(list, unsubscriptionDelegate, handler);
		}

		public void ListenEventListElementChange<TType>
			(IReactiveListReadOnly<TType> list, EventHandler<GenericPairEventArgs<int, TType>> handler)
		{
			ListenEventListElementChange(list as ReactiveList<TType>, handler);
		}

		public void ListenEventListRemoveItem<TType>
			(ReactiveList<TType> list, EventHandler<GenericPairEventArgs<int, TType>> handler)
		{
			list.OnRemoveItem += handler;
			Action unsubscriptionDelegate = () => list.OnRemoveItem -= handler;
			AddSubscription(list, unsubscriptionDelegate, handler);
		}

		public void ListenEventListRemoveItem<TType>
			(IReactiveListReadOnly<TType> list, EventHandler<GenericPairEventArgs<int, TType>> handler)
		{
			list.OnRemoveItem += handler;
			Action unsubscriptionDelegate = () => list.OnRemoveItem -= handler;
			AddSubscription(list, unsubscriptionDelegate, handler);
		}

		public void ListenEventListRemoveRange<TType>
			(ReactiveList<TType> list, EventHandler<ReactiveListRemoveRangeArgs<TType>> handler)
		{
			list.OnRemoveRange += handler;
			Action unsubscriptionDelegate = () => list.OnRemoveRange -= handler;
			AddSubscription(list, unsubscriptionDelegate, handler);
		}

		public void UnlistenEventListAddRange<TType>
			(ReactiveList<TType> list, EventHandler<GenericEventArg<IEnumerable<TType>>> handler)
		{
			RemoveSubscription(list, handler);
		}

		public void UnlistenEventListAddElement<TType>
			(ReactiveList<TType> list, EventHandler<GenericPairEventArgs<int, TType>> handler)
		{
			RemoveSubscription(list, handler);
		}

		public void UnlistenEventListElementChange<TType>
			(ReactiveList<TType> list, EventHandler<GenericPairEventArgs<int, TType>> handler)
		{
			RemoveSubscription(list, handler);
		}

		public void UnlistenEventListRemoveItem<TType>
			(ReactiveList<TType> list, EventHandler<GenericPairEventArgs<int, TType>> handler)
		{
			RemoveSubscription(list, handler);
		}

		public void UnlistenEventListRemoveRange<TType>
			(ReactiveList<TType> list, EventHandler<ReactiveListRemoveRangeArgs<TType>> handler)
		{
			RemoveSubscription(list, handler);
		}

		// public void UnlistenEvent<TType>
		// 	(IReactiveProperty<TType> property, EventHandler<GenericEventArg<TType>> handler)
		// {
		// 	RemoveSubscription(property, handler);
		// }

		public void UnlistenEvent<TType>
			(IReactivePropertyReadonly<TType> property, EventHandler<GenericEventArg<TType>> handler)
		{
			RemoveSubscription(property, handler);
		}

		public void UnlistenEvent<TType>
			(IReactivePropertyReadonly<TType> property, EventHandler<PropertyEventArgs<TType>> handler)
		{
			RemoveSubscription(property, handler);
		}

		public void UnlistenEvent<TType>
			(
			UnityEvent<TType> eventObject,
			UnityAction<TType> handler
			)
		{
			RemoveSubscription(eventObject, handler);
		}

		public void UnlistenEvent<TType, TType2>
			(
			UnityEvent<TType, TType2> eventObject,
			UnityAction<TType, TType2> handler
			)
		{
			RemoveSubscription(eventObject, handler);
		}

		public void UnlistenEvent<TType, TType2, TType3>
			(
			UnityEvent<TType, TType2, TType3> eventObject,
			UnityAction<TType, TType2, TType3> handler
			)
		{
			RemoveSubscription(eventObject, handler);
		}

		public void ListenEventDictionaryAddItem<TKey, TValue>
			(
			IReactiveDictionaryReadOnly<TKey, TValue> dictionary,
			EventHandler<GenericPairEventArgs<TKey, TValue>> handler,
			bool invokeAfterSubscribe = false
			)
		{
			ListenEventDictionaryAddItem((ReactiveDictionary<TKey, TValue>) dictionary, handler, invokeAfterSubscribe);
		}

		public void ListenEventDictionaryAddItem<TKey, TValue>
			(
			ReactiveDictionary<TKey, TValue> dictionary,
			EventHandler<GenericPairEventArgs<TKey, TValue>> handler,
			bool invokeAfterSubscribe = false
			)
		{
			dictionary.OnAddItem += handler;
			Action unsubscriptionDelegate = () => dictionary.OnAddItem -= handler;
			AddSubscription(dictionary, unsubscriptionDelegate, handler);

			if (invokeAfterSubscribe)
			{
				var args = new GenericPairEventArgs<TKey, TValue>();

				foreach (var value in dictionary)
				{
					args.Key = value.Key;
					args.Value = value.Value;
					handler(dictionary, args);
				}
			}
		}

		public void ListenEventDictionaryRemoveItem<TKey, TValue>
			(IReactiveDictionaryReadOnly<TKey, TValue> dictionary, EventHandler<GenericPairEventArgs<TKey, TValue>> handler)
		{
			ListenEventDictionaryRemoveItem((ReactiveDictionary<TKey, TValue>) dictionary, handler);
		}

		public void ListenEventDictionaryRemoveItem<TKey, TValue>
			(ReactiveDictionary<TKey, TValue> dictionary, EventHandler<GenericPairEventArgs<TKey, TValue>> handler)
		{
			dictionary.OnRemoveItem += handler;
			Action unsubscriptionDelegate = () => dictionary.OnRemoveItem -= handler;
			AddSubscription(dictionary, unsubscriptionDelegate, handler);
		}

		public void ListenEventDictionaryElementChange<TKey, TValue>
			(
			IReactiveDictionaryReadOnly<TKey, TValue> dictionary,
			EventHandler<GenericPairEventArgs<TKey, TValue>> handler,
			bool invokeAfterSubscribe = false
			)
		{
			ListenEventDictionaryElementChange((ReactiveDictionary<TKey, TValue>) dictionary, handler, invokeAfterSubscribe);
		}

		public void ListenEventDictionaryElementChange<TKey, TValue>
			(
			ReactiveDictionary<TKey, TValue> dictionary,
			EventHandler<GenericPairEventArgs<TKey, TValue>> handler,
			bool invokeAfterSubscribe = false
			)
		{
			dictionary.OnElementChange += handler;
			Action unsubscriptionDelegate = () => dictionary.OnElementChange -= handler;
			AddSubscription(dictionary, unsubscriptionDelegate, handler);

			if (invokeAfterSubscribe)
			{
				var args = new GenericPairEventArgs<TKey, TValue>();

				foreach (var value in dictionary)
				{
					args.Key = value.Key;
					args.Value = value.Value;
					handler(dictionary, args);
				}
			}
		}

		public void ListenEventDictionaryAddRange<TKey, TValue>
			(
			IReactiveDictionaryReadOnly<TKey, TValue> dictionary,
			EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> handler,
			IDictionary<TKey, TValue> defaultValue = null
			)
		{
			ListenEventDictionaryAddRange((ReactiveDictionary<TKey, TValue>) dictionary, handler, defaultValue);
		}

		public void ListenEventDictionaryAddRange<TKey, TValue>
			(
			ReactiveDictionary<TKey, TValue> dictionary,
			EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> handler,
			IDictionary<TKey, TValue> defaultValue = null
			)
		{
			dictionary.OnAddRange += handler;
			Action unsubscriptionDelegate = () => dictionary.OnAddRange -= handler;
			AddSubscription(dictionary, unsubscriptionDelegate, handler);
			if (defaultValue != null) handler(dictionary, new GenericEventArg<IDictionary<TKey, TValue>>(defaultValue));
		}

		public void ListenEventDictionaryClear<TKey, TValue>
			(
			IReactiveDictionaryReadOnly<TKey, TValue> dictionary,
			EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> handler
			)
		{
			ListenEventDictionaryClear((ReactiveDictionary<TKey, TValue>) dictionary, handler);
		}

		public void ListenEventDictionaryClear<TKey, TValue>
			(
			ReactiveDictionary<TKey, TValue> dictionary,
			EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> handler
			)
		{
			dictionary.OnClear += handler;
			Action unsubscriptionDelegate = () => dictionary.OnClear -= handler;
			AddSubscription(dictionary, unsubscriptionDelegate, handler);
		}

		public void UnlistenEventDictionaryAddItem<TKey, TValue>
			(IReactiveDictionaryReadOnly<TKey, TValue> dictionary, EventHandler<GenericPairEventArgs<TKey, TValue>> handler)
		{
			UnlistenEventDictionaryAddItem((ReactiveDictionary<TKey, TValue>) dictionary, handler);
		}

		public void UnlistenEventDictionaryAddItem<TKey, TValue>
			(ReactiveDictionary<TKey, TValue> dictionary, EventHandler<GenericPairEventArgs<TKey, TValue>> handler)
		{
			RemoveSubscription(dictionary, handler);
		}

		public void UnlistenEventDictionaryRemoveItem<TKey, TValue>
			(IReactiveDictionaryReadOnly<TKey, TValue> dictionary, EventHandler<GenericPairEventArgs<TKey, TValue>> handler)
		{
			UnlistenEventDictionaryRemoveItem((ReactiveDictionary<TKey, TValue>) dictionary, handler);
		}

		public void UnlistenEventDictionaryRemoveItem<TKey, TValue>
			(ReactiveDictionary<TKey, TValue> dictionary, EventHandler<GenericPairEventArgs<TKey, TValue>> handler)
		{
			RemoveSubscription(dictionary, handler);
		}

		public void UnlistenEventDictionaryElementChange<TKey, TValue>
			(IReactiveDictionaryReadOnly<TKey, TValue> dictionary, EventHandler<GenericPairEventArgs<TKey, TValue>> handler)
		{
			UnlistenEventDictionaryElementChange((ReactiveDictionary<TKey, TValue>) dictionary, handler);
		}

		public void UnlistenEventDictionaryElementChange<TKey, TValue>
			(ReactiveDictionary<TKey, TValue> dictionary, EventHandler<GenericPairEventArgs<TKey, TValue>> handler)
		{
			RemoveSubscription(dictionary, handler);
		}

		public void UnlistenEventDictionaryAddRange<TKey, TValue>
			(
			IReactiveDictionaryReadOnly<TKey, TValue> dictionary,
			EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> handler
			)
		{
			UnlistenEventDictionaryAddRange((ReactiveDictionary<TKey, TValue>) dictionary, handler);
		}

		public void UnlistenEventDictionaryAddRange<TKey, TValue>
			(
			ReactiveDictionary<TKey, TValue> dictionary,
			EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> handler
			)
		{
			RemoveSubscription(dictionary, handler);
		}

		public void UnlistenEventDictionaryClear<TKey, TValue>
			(
			IReactiveDictionaryReadOnly<TKey, TValue> dictionary,
			EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> handler
			)
		{
			UnlistenEventDictionaryClear((ReactiveDictionary<TKey, TValue>) dictionary, handler);
		}

		public void UnlistenEventDictionaryClear<TKey, TValue>
			(
			ReactiveDictionary<TKey, TValue> dictionary,
			EventHandler<GenericEventArg<IDictionary<TKey, TValue>>> handler
			)
		{
			RemoveSubscription(dictionary, handler);
		}

		public void ListenEventListClear<TType>
			(ReactiveList<TType> list, EventHandler<GenericEventArg<IEnumerable<TType>>> handler)
		{
			list.OnClear += handler;
			Action unsubscriptionDelegate = () => list.OnClear -= handler;
			AddSubscription(list, unsubscriptionDelegate, handler);
		}

		public void ListenEventListClear<TType>
			(IReactiveListReadOnly<TType> list, EventHandler<GenericEventArg<IEnumerable<TType>>> handler)
		{
			list.OnClear += handler;
			Action unsubscriptionDelegate = () => list.OnClear -= handler;
			AddSubscription(list, unsubscriptionDelegate, handler);
		}

		public void ListenEventListSort<TType>
			(ReactiveList<TType> list, EventHandler<ReactiveListSortingArgs<TType>> handler)
		{
			list.OnSort += handler;
			Action unsubscribeDelegate = () => list.OnSort -= handler;
			AddSubscription(list, unsubscribeDelegate, handler);
		}

		public void UnlistenEventListClear<TType>
			(ReactiveList<TType> list, EventHandler<GenericEventArg<IEnumerable<TType>>> handler)
		{
			RemoveSubscription(list, handler);
		}

		public void AddToRedispatchMap<TType>
			(
			ReactiveProperty<TType> propertySource,
			ReactiveProperty<TType> propertyRepeater,
			bool invokeAfterSubscribe = false
			)
		{
			ListenEvent((IReactivePropertyReadonly<TType>) propertySource,
						(sender, arg) => propertyRepeater.Value = arg.Value);

			if (invokeAfterSubscribe) propertyRepeater.Value = propertySource.Value;
		}
		#endregion

		#region Protected Members
		protected void RemoveSubscription(object notifyContainer, Delegate handler)
		{
			if (_subscriptions.ContainsKey(notifyContainer))
			{
				var propertySubscriptions = _subscriptions[notifyContainer];

				for (var i = propertySubscriptions.Count - 1; i > -1; i--)
				{
					var currentSubscription = propertySubscriptions[i];

					if (handler != null && currentSubscription.OriginalHandler == handler)
					{
						currentSubscription.UnsubscribeHandler();
						propertySubscriptions.RemoveAt(i);
					}
				}
			}
		}

		protected void AddSubscription(object notifyContainer, Action unsubscribeAction, Delegate originalhandler)
		{
			if (!_subscriptions.ContainsKey(notifyContainer))
				_subscriptions[notifyContainer] = new List<Subscription>();

			var newSubscriptions = new Subscription(originalhandler, unsubscribeAction);
			_subscriptions[notifyContainer].Add(newSubscriptions);
		}
		#endregion
	}
}