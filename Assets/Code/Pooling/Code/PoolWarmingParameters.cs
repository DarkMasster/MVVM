using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DM.Pooling
{
	/// <summary>
	/// Объект содержит пары <see cref="KeyCapacityPair"/>. Используется для прогрева и очистки пула.
	/// </summary>
	public class PoolWarmingParameters : IEnumerable<KeyCapacityPair>, IDictionary<string, int>
	{
		#region Properties
		public int Count => _parameters.Count;
		public bool IsReadOnly => _parameters.IsReadOnly;

		public int this[string key] { get => _parameters[key]; set => _parameters[key] = value; }
		public ICollection<string> Keys => _parameters.Keys;
		public ICollection<int> Values => _parameters.Values;
		#endregion

		#region Private Fields
		private readonly IDictionary<string, int> _parameters = new Dictionary<string, int>();
		#endregion

		#region Interface Implementations
		IEnumerator<KeyValuePair<string, int>> IEnumerable<KeyValuePair<string, int>>.GetEnumerator() => _parameters.GetEnumerator();

		public IEnumerator<KeyCapacityPair> GetEnumerator() => _parameters.Select(p => new KeyCapacityPair
		{
			Key = p.Key,
			Capacity = p.Value
		}).GetEnumerator();

		public void Add(KeyValuePair<string, int> item)
		{
			_parameters.Add(item);
		}

		public void Clear()
		{
			_parameters.Clear();
		}

		public bool Contains(KeyValuePair<string, int> item) => _parameters.Contains(item);

		public void CopyTo(KeyValuePair<string, int>[] array, int arrayIndex)
		{
			_parameters.CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<string, int> item) => _parameters.Remove(item);

		public void Add(string key, int value)
		{
			_parameters.Add(key, value);
		}

		public bool ContainsKey(string key) => _parameters.ContainsKey(key);

		public bool Remove(string key) => _parameters.Remove(key);

		public bool TryGetValue(string key, out int value) => _parameters.TryGetValue(key, out value);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		#endregion
	}
}