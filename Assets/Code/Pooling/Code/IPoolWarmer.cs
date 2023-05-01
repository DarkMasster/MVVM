using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace DM.Pooling
{
	public interface IPoolWarmer
	{
		#region Properties
		/// <summary>
		///     Отчет о состоянии пула
		/// </summary>
		PoolInfo PoolInfo { get; }
		#endregion

		#region Public Members
		/// <summary>
		///     Метод создает объекты для каждого переданного ключа, так, чтобы их количество в пуле было не меньше чем переданное значение. Если не удется
		///     разрешить ключ, то генерируется <see cref="PoolReleasingException" />. При вызове во время выполнения прогрев генерируется <see cref="PoolInvalidAccessExceptioon" />.
		/// </summary>
		/// <param name="parameters">Объект содержащий ключи и количество объектов которое должно находиться в пуле по завершению прогрева</param>
		/// <param name="progress">Прогресс.</param>
		/// <returns>Состояние пула после прогрева.</returns>
		UniTask<PoolInfo> PreWarmPool(PoolWarmingParameters parameters, IProgress<float> progress = null);

		/// <summary>
		///     Метод удаляет из рула заданное количество объектов. Если в пуле не находятся объекты с заданым ключем генерируется
		///     <see cref="PoolReleasingException" />. При вызове во время выполнения прогрев генерируется <see cref="PoolInvalidAccessExceptioon" />.
		/// </summary>
		/// <param name="parameters">Объект содержащий ключи и количество объектов которые необходимо удалить.</param>
		/// <param name="progress">Прогресс.</param>
		/// <returns>Состояние пула после завершения очистки.</returns>
		UniTask<PoolInfo> ReleasePool(PoolWarmingParameters parameters, IProgress<float> progress = null);

		/// <summary>
		///     Метод удаляет объекты из пула по заданным ключам. Если в пуле не находятся объекты с заданым ключем генерируется
		///     <see cref="PoolReleasingException" />. При вызове во время выполнения прогрев генерируется <see cref="PoolInvalidAccessExceptioon" />.
		/// </summary>
		/// <param name="keys"></param>
		/// <param name="progress"></param>
		/// <returns>Состояние пула после завершения очистки.</returns>
		UniTask<PoolInfo> ReleasePool(IEnumerable<string> keys = null, IProgress<float> progress = null);
		#endregion
	}
}