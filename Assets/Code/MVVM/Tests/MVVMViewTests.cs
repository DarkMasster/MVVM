using System;
using System.Collections;
using System.Reflection;
using DM.MVVM.View;
using Cysharp.Threading.Tasks;
using MVVM.Tests.Polymorfic.View;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace DM.MVVM.Test
{
	public class MVVMViewTests
	{
		#region Constants
		private const int WaitMilliseconds = 1000;
		#endregion

		#region Public Members
		[UnityTest]
		public IEnumerator ViewFacadeFactoryKeyNotResolvedExceptionTest()
		{
			return ViewFacadeFactoryKeyNotResolvedExceptionTestInternal().ToCoroutine();

			async UniTask ViewFacadeFactoryKeyNotResolvedExceptionTestInternal()
			{
				var facadeFactory = new TestViewFacadeFactory();
				const string wrongKey = "wrong key";

				var thrown = false;
				
				try
				{
					await facadeFactory.Get<TestViewFacade>(wrongKey);
				}
				catch (KeyNotResolvedException e)
				{
					thrown = true;
				}
				Assert.AreEqual(true, thrown);
			}
		}

		[UnityTest]
		public IEnumerator ViewLogicFactoryIncompatibleParametersExceptionTest()
		{
			return ViewLogicFactoryIncompatibleParametersExceptionTestInternal().ToCoroutine();

			async UniTask ViewLogicFactoryIncompatibleParametersExceptionTestInternal()
			{
				var facadeFactory = CreateFacadeFactory();
				var logicFactory = CreateLogicFactory(facadeFactory);
				var model = new TestModel();
				var viewModel = new TestViewModel(model);

				var viewFacade = await facadeFactory.Get(nameof(TestViewFacade));

				Assert.Catch(() =>
				{
					var viewLogic = logicFactory.Create<RandomItemsViewLogic>(typeof(RandomItemsViewLogic), viewModel, viewFacade);
				});
			}
		}

		[UnityTest]
		public IEnumerator BasicTest()
		{
			var task = BasicTestInternal();

			return task.ToCoroutine();

			async UniTask BasicTestInternal()
			{
				PrepareScene();
				var facadeFactory = CreateFacadeFactory();
				var logicFactory = CreateLogicFactory(facadeFactory);

				var model = new TestModel();
				var viewModel = new TestViewModel(model);
				var viewFacade = await facadeFactory.Get(nameof(TestViewFacade));
				var viewLogic = logicFactory.Create<TestViewLogic>(typeof(TestViewLogic), viewModel, viewFacade);

				viewLogic.SetViewFacadeParent(null);

				await viewLogic.Initialize();

				var facade = (TestViewFacade) viewLogic.GetType().GetProperty("ViewFacade", BindingFlags.Instance | BindingFlags.NonPublic).
														GetValue(viewLogic);

				Assert.AreEqual("0", facade.Text.text);

				await UniTask.Delay(WaitMilliseconds);

				facade.Button.onClick.Invoke();

				await UniTask.Delay(WaitMilliseconds);

				facade.Button.onClick.Invoke();

				await UniTask.Delay(WaitMilliseconds);

				facade.Button.onClick.Invoke();

				await UniTask.Delay(WaitMilliseconds);

				Assert.AreEqual("3", facade.Text.text);

				viewLogic.DeInitialize();

				Object.Destroy(facade.gameObject);

				await UniTask.Delay(WaitMilliseconds);

				HandleTestEnded();
			}
		}

		[UnityTest]
		public IEnumerator DynamicNestedViewLogicInstantiation()
		{
			return DynamicNestedViewLogicInstantiationInternal().ToCoroutine();

			async UniTask DynamicNestedViewLogicInstantiationInternal()
			{
				PrepareScene();

				var facadeFactory = CreateFacadeFactory();
				var logicFactory = CreateLogicFactory(facadeFactory);
				var viewFacade = await facadeFactory.Get(nameof(DynamicViewFacade));
				var model = new RandomItemsModel();
				var viewModel = new RandomItemsViewModel(model);
				var viewLogic = logicFactory.Create<RandomItemsViewLogic>(typeof(RandomItemsViewLogic), viewModel, viewFacade);

				viewLogic.SetViewFacadeParent(null);

				await viewLogic.Initialize();

				await UniTask.Delay(WaitMilliseconds);

				viewLogic.DeInitialize();

				HandleTestEnded();
			}
		}

		[UnityTest]
		public IEnumerator StaticNestedViewLogicInstantiation()
		{
			return StaticNestedViewLogicInstantiationInternal().ToCoroutine();

			async UniTask StaticNestedViewLogicInstantiationInternal()
			{
				PrepareScene();

				var facadeFactory = CreateFacadeFactory();
				var logicFactory = CreateLogicFactory(facadeFactory);
				var viewFacade = await facadeFactory.Get(nameof(StaticViewFacade));
				var model = new StaticModel();
				var viewModel = new StaticViewModel(model);
				var viewLogic = logicFactory.Create<StaticViewLogic>(typeof(StaticViewLogic), viewModel, viewFacade);
				viewLogic.SetViewFacadeParent(null);
				await viewLogic.Initialize();

				await UniTask.Delay(WaitMilliseconds);

				viewLogic.DeInitialize();

				HandleTestEnded();
			}
		}

		[UnityTest]
		public IEnumerator PolymorphicModels()
		{
			return PolymorphicModelsInternal().ToCoroutine();

			async UniTask PolymorphicModelsInternal()
			{
				PrepareScene();
				var facadeFactory = CreateFacadeFactory();
				var logicFactory = CreateLogicFactory(facadeFactory);

				var viewFacade = await facadeFactory.Get(nameof(ItemsHolderViewFacade));

				var model = new ItemsHolderModel();
				var viewModel = new ItemsHolderViewModel(model);

				var viewLogic =
					logicFactory.Create<ItemsHolderViewLogic>(typeof(ItemsHolderViewLogic), viewModel, viewFacade);

				viewLogic.SetViewFacadeParent(null);
				await viewLogic.Initialize();

				await UniTask.Delay(WaitMilliseconds);

				HandleTestEnded();
			}
		}
		#endregion

		#region Private Members
		private IViewLogicFactory CreateLogicFactory(IViewFacadeFactory facadeFactory)
		{
			var logicFactory = new TestViewLogicFactory(facadeFactory);

			return logicFactory;
		}

		private IViewFacadeFactory CreateFacadeFactory()
		{
			var facadeFactory = new TestViewFacadeFactory();

			return facadeFactory;
		}

		private void PrepareScene()
		{
			var camera = new GameObject("Camera").AddComponent<Camera>();
		}

		private void HandleTestEnded()
		{
			var camera = Object.FindObjectOfType<Camera>();
			Object.Destroy(camera.gameObject);

			var stashGo = GameObject.Find("Stash");
			Object.Destroy(stashGo);
		}
		#endregion
	}
}