using Cysharp.Threading.Tasks;

namespace DM.Pooling
{
	public interface IPoolGetter<T>
	{
		#region Public Members
		/// <summary>
		///     Метод извлекает объект из пула по заданному ключу.
		/// </summary>
		/// <param name="key">Ключ объекта.</param>
		/// <returns>Объект соответствующи ключу.</returns>
		UniTask<T> Get(string key);

		void ReturnToPool(T obj);
		#endregion
	}
}