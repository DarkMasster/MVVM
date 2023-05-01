using System;

namespace DM.ReactiveTypes
{
	public interface IReactiveProperty< T >
	{
		#region Events
		event EventHandler< GenericEventArg< T > > OnValueChanged;
		event EventHandler< PropertyEventArgs< T > > OnValueChangedExtended;
		#endregion

		#region Public Members
		T GetValue();
		#endregion
	}

	public interface IReactivePropertyReadonly< T >
	{
		#region Properties
		T Value { get; }
		#endregion

		#region Events
		event EventHandler< GenericEventArg< T > > OnValueChanged;
		event EventHandler< PropertyEventArgs< T > > OnValueChangedExtended;
		#endregion
	}

	public enum TypeDispatchEventMode
	{
		Always,
		ValueChange
	}

	public class PropertyEventArgs< T > : EventArgs
	{
		#region Public Fields
		public T OldValue;
		public T NewValue;
		#endregion

		#region Constructors
		public PropertyEventArgs()
		{
		}

		public PropertyEventArgs( T oldValue, T newValue )
		{
			OldValue = oldValue;
			NewValue = newValue;
		}
		#endregion
	}

	public class ReactiveProperty< T > : IReactiveProperty< T >, IReactivePropertyReadonly< T >
	{
		#region Private Fields
		private T _value;
		private GenericEventArg< T > _args;
		private PropertyEventArgs< T > _extendedArgs;
		private object _sender;
		private TypeDispatchEventMode _dispatchEventMode;
		#endregion

		#region Properties
		public T Value
		{
			get
			{
				return _value;
			}
			set
			{
				if( _value == null || !_value.Equals( value ) || _dispatchEventMode == TypeDispatchEventMode.Always )
				{
					_extendedArgs.OldValue = _value;
					_value = value;
					_extendedArgs.NewValue = _value;
					_args.Value = _value;

					if( OnValueChanged != null )
					{
						OnValueChanged( _sender ?? this, _args );
					}
					if( OnValueChangedExtended != null )
					{
						OnValueChangedExtended( _sender ?? this, _extendedArgs );
					}
				}
			}
		}
		#endregion

		#region Events
		public event EventHandler< GenericEventArg< T > > OnValueChanged;
		public event EventHandler< PropertyEventArgs< T > > OnValueChangedExtended;
		#endregion

		#region Constructors
		public ReactiveProperty( TypeDispatchEventMode dispatchEventMode = TypeDispatchEventMode.ValueChange )
		{
			_args = new GenericEventArg< T >( default( T ) );
			_extendedArgs = new PropertyEventArgs< T >( default( T ), default( T ) );
			_dispatchEventMode = dispatchEventMode;
		}

		public ReactiveProperty( T value, TypeDispatchEventMode dispatchEventMode = TypeDispatchEventMode.ValueChange ) : this()
		{
			_value = value;
			_dispatchEventMode = dispatchEventMode;
		}

		public ReactiveProperty( T value, object sender, TypeDispatchEventMode dispatchEventMode = TypeDispatchEventMode.ValueChange ) : this( value )
		{
			_sender = sender;
			_dispatchEventMode = dispatchEventMode;
		}
		#endregion

		#region Public Members
		public void HandleValueChanged( object sender, GenericEventArg< T > valueArg )
		{
			if( sender == _sender )
			{
				throw new Exception( "Self event subscribtion pohibited" );
			}

			Value = valueArg.Value;
		}

		public void SetValue( T value )
		{
			if( value is T )
			{
				_value = value;
			}
		}

		public T GetValue()
		{
			return _value;
		}

		public override string ToString()
		{
			return _value != null ? _value.ToString() : "NULL value";
		}

		public static implicit operator T( ReactiveProperty< T > value )
		{
			return value.GetValue();
		}
		#endregion
	}
}
