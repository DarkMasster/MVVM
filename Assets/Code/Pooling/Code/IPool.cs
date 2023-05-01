namespace DM.Pooling
{
	public interface IPool<T> : IPoolWarmer, IPoolGetter<T>
	{
	}
}