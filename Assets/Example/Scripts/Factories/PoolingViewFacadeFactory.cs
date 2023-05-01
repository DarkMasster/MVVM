using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DM.MVVM.View;
using DM.Pooling;
using UnityEngine;

namespace DM.UI
{
	public class PoolingViewFacadeFactory : IPoolWarmer, IViewFacadeFactory
	{
		#region Properties
		public PoolInfo PoolInfo => _pool.PoolInfo;
		#endregion

		#region Private Fields
		private readonly IPool<GameObject> _pool;
		#endregion

		#region Constructors
		public PoolingViewFacadeFactory(IPool<GameObject> pool) => _pool = pool;
		#endregion

		#region Interface Implementations
		public async UniTask<PoolInfo> PreWarmPool
			(PoolWarmingParameters keyCapacityPairs, IProgress<float> progress = null) => await _pool.PreWarmPool(keyCapacityPairs, progress);

		public async UniTask<PoolInfo> ReleasePool
			(PoolWarmingParameters keyCapacityPairs, IProgress<float> progress = null) => await _pool.ReleasePool(keyCapacityPairs, progress);

		public async UniTask<PoolInfo> ReleasePool
			(IEnumerable<string> keys = null, IProgress<float> progress = null) => await _pool.ReleasePool(keys, progress);

		public async UniTask<T> Get<T>(string key) where T : ViewFacade => (T) await Get(key);

		public async UniTask<ViewFacade> Get(string key)
		{
			var facadeGo = await _pool.Get(key);

			var facade = facadeGo.GetComponent<ViewFacade>();

			return facade;
		}

		public void Release(ViewFacade viewFacade)
		{
			_pool.ReturnToPool(viewFacade.gameObject);
		}
		#endregion
	}
}