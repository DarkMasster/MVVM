using System;

namespace DM.ReactiveTypes
{
    public class GenericEventArg< TValue > : EventArgs
    {
        #region Public Fields
        public TValue Value;
        #endregion

        #region Constructors
        public GenericEventArg()
        {
        }

        public GenericEventArg( TValue value )
        {
            Value = value;
        }
        #endregion
    }
}
