using Cysharp.Threading.Tasks;
using DM.ReactiveTypes;
using UnityEngine;

public class TestItem
{
	#region Properties
	public int Value { get; }
	#endregion

	#region Constructors
	public TestItem() => Value = Random.Range(0, 100);
	#endregion
}

public class RandomItemsModel
{
	#region Properties
	public IReactiveListReadOnly<TestItem> Items => _items;
	#endregion

	#region Private Fields
	private readonly ReactiveList<TestItem> _items;
	#endregion

	#region Constructors
	public RandomItemsModel()
	{
		_items = new ReactiveList<TestItem>
		{
			new TestItem(),
			new TestItem(),
			new TestItem()
		};

		MutateList();
	}
	#endregion

	#region Private Members
	private async void MutateList()
	{
		while (true)
		{
			var mutation = Random.Range(0, 2);

			switch (mutation)
			{
				case 0:
					var index = Random.Range(0, _items.Count);

					if (_items.Count > 0)
						_items.RemoveAt(index);

					break;

				case 1:
					_items.Add(new TestItem());

					break;
			}

			await UniTask.Delay(2000);
		}
	}
	#endregion
}