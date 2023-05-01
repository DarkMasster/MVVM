namespace DM.ReactiveTypes
{
    public class GenericPairEventArgs< TKey, TValue > : GenericEventArg< TValue >
    {
        #region Public Fields
        public TKey Key;
        #endregion

        #region Constructors
        public GenericPairEventArgs()
        {
        }

        public GenericPairEventArgs( TKey key, TValue value ) : base( value )
        {
            Key = key;
        }
        #endregion
    }
}
