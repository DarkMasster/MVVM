using Cysharp.Threading.Tasks;

namespace DM.MVVM.View
{
	public interface IViewFacadeFactory
	{
		#region Public Members
		UniTask<T> Get<T>(string key) where T : ViewFacade;

		UniTask<ViewFacade> Get(string key);

		void Release(ViewFacade viewFacade);
		#endregion
	}
}