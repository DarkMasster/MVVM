using Cysharp.Threading.Tasks;

namespace DM.Pooling
{
	public interface IPrefabsProvider<T>
	{
		#region Public Members
		UniTask<T> Get(string key);
		void Release(string key);
		#endregion
	}
}