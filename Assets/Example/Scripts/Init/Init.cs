using Cysharp.Threading.Tasks;
using DM.Example.Data;
using DM.Example.Factories;
using DM.Example.Services.MapService;
using DM.Example.Views;
using DM.Example.Views.UI;
using DM.MVVM.View;
using DM.Pooling;
using DM.UI;
using UnityEngine;
using Zenject;

/*
Предпосылки:
	1. Высокая связанность кода. Все в перемежку - логика, отображение, данные.
	3. Высокие трудозатраты на расширения и рефакторинг, так-как разные реализации в разных частей кода проекта из-за 
	   отсуствия единого архитектурного подхода.
	4. Нет возможности распараллеивать работу на нескольких программистов. 

   
   
   
Идея:
	Реализовать архитектуру, которая бы строго разделял проект на слабо связанные слои и при этом поддержать возможсть паралельной разработки, расширения и рефаторинга этих слоев с минимальной зависимостью.   
 
Реализация в общем:
	Реализованн паттерн MVVM, который представлен следующими сушностями:
		1. Слой ViewModel - абстрагирует работу с данными. Его задача сконвертировать данные бизнес логики в данные отображения. Являетсья связующим звеном между бизнес логикой и отображением. Его поля реактивные. Может содержать дочерние ViewModel-и.  
		2. Слой View - разделен на ViewLogic и ViewFacade, которые полностью инкапсулируют всю работу отображения. 
			a. ViewFacade - наследник MonoBehaviuor, не содержит ни какой логики и предоставляет доступ к сериализованным полям и компонентам GameObject-a. Пулиться в GameObjectPool. 
			b. ViewLogic - свзяывет ViewFacade и ViewModel. Подписываясь на события изменния данных ViewModel. Реализует визуальное поведение и обеспечивает жизненный цикл и обработку внешних события ( user input, события Unity ) которые передает ViewModel. Может содержать дочерние ViewLogic-и
		3. Модель данных - данных бизнес-логики,  может быть представленна в любом виде.
	
	Все сущности и сервисы реализовынны с использованием интерфейсов, что бы можно было гибко адаптировать под кастомные задча. Все разбито на acmdef-ы.
	
	
	           
	   
Плюсы - так как слои бизнес-логики и отображения четко разделенны, это позволяет:
	1. Избежат связанности кода. Единственная свзяующая точка - это ViewModel.
	2. Все задачи решаютсья по одному шаблону. UI, Meta, Core gameplay. 
	3. Вести работу параллено. Можно полностью реализовать визуальную логику и поведение не привязываясь к тому как именно бизнес-логика отработает ( локально иди на сервере ). 
	4. Быстро вностить изменния минимально задевая другие слои. Можно рефакторить слои практически независимо.
	5. Удобно покрывать тестами,  писать читы и тулзы.

Минусы:
	1. Треубеться писать большее количество кода. Особено для это заметно на простых вещах ( можно рещить кодо-генерацией ).
	2. На начальном этапе - более высокий порог вхождения. Нужно понять и разобратсья с данной архиетктурой.
	3. Требуеться больше времени при планировании новых задач, так при реализации как важно декрмпозировать задачу на большее кодичество сущностей.
	
	
	
	
Структура проекта:

	1. Code - содержить базовые реализации необходимых сущностей.
		a. MVVM - реализация паттерна.
		b. Subscriber - использеться для цетролизованнаой рабоыт с событиями. Подписка\отписка.
		c. Pooling - простая реализация пулла.
		d. ReactiveTypes - реализация реактивщины для свойст и коллекций.
		
	2. Example - простой пример для демонстрации. Пример приметивный, не показывает всех возможностей, но демонстриует подход в общем.
		a. Resources - там лежат префабы. Карта и несколько объектов.
		b. Scripts/Factories - фабрики классов.
			I. PoolingViewFacadeFactory - простой пример создания ViewFacade-ов и их пулинга.
			II. ViewFacadesPrefabsProvider - простой пример доступа к префаба с кешированием. В реальном проекте работает с бандалми.
			III. ViewLogicFactory - простой пример создания ViewLogic.
		c. Scripts/Init - основаня точка инициализации. В качестве DI используеться Zenject. В методе Initialize() - пример создания и работы.
		d. Scripts/Services - простой сервис бизнес-логики работы с картой.
			I. MapModel, MapObjectData - максимально примтивное представление данных карты. Специально реализованны такими для демонастрации отличия данных бизнес-логики от данных отображения.
			II. MapService - реализует работу с картой,  создание, удаление, изменение элементов карты. Реактивность реализованна примитивным образом специально.
		e. Scripts/View - содержит реализацию сущностей MVVM.
			I. Map - ViewLogicMap, создает и дочерние элементы объектов, как на старте при инициализации, так и динамически при изменени данных. Для простоты соответсвие типов объектов карты  и префабов реализованно через статическую карту.  
			II. MapObject - примитивная реализация объектоа карты, реагирует на изменения позиции.
*/

namespace DM.Example
{
	public class Init : MonoInstaller
	{
		#region UnitySerialized
		[SerializeField] private ViewFacadeUIMapInfo _viewFacadeUIMapInfo;
		#endregion

		#region Private Members
		private async UniTaskVoid Initialize()
		{
			var mapService = Container.Resolve<IMapService>();
			var viewFacadeFactory = Container.Resolve<IViewFacadeFactory>();
			var viewLogicFactory = Container.Resolve<IViewLogicFactory>();

			var trackedObjectID = mapService.CreateObject(0, 0, EMapObjectType.TypeB);
			mapService.CreateObject(3, -1, EMapObjectType.TypeA);
			mapService.CreateObject(-3, 1, EMapObjectType.TypeC);

			// Создаем 3D вьюху -  динамически
			{
				var viewFacade = await viewFacadeFactory.Get("ViewFacadeMap");
				var viewModel = new ViewModelMap(mapService);
				var viewLogic = viewLogicFactory.Create<ViewLogicMap>(viewModel, viewFacade);
				viewLogic.SetViewFacadeParent(null);
				await viewLogic.Initialize();
			}

			// Создаем UI - статически
			{
				var viewModel = new ViewModelMap(mapService);
				var viewLogic = viewLogicFactory.Create<ViewLogicUIMapInfo>(viewModel, _viewFacadeUIMapInfo);
				await viewLogic.Initialize();
			}

			await UniTask.Delay(500);
			mapService.CreateObject(-5, 1, EMapObjectType.Simple);

			await UniTask.Delay(500);
			mapService.CreateObject(5, 1, EMapObjectType.TypeA);

			await UniTask.Delay(500);

			if (!mapService.IsObjectExist(trackedObjectID)) return;

			for (var i = 0; i < 10; i++)
			{
				mapService.SetObjectPosition(trackedObjectID, 0, i);
				await UniTask.Delay(100);
			}

			await UniTask.Delay(500);
			mapService.RemoveObject(trackedObjectID);
		}
		#endregion

		#region Overrides
		public override void InstallBindings()
		{
			var stashGo = new GameObject("-PoolStash");
			DontDestroyOnLoad(stashGo);

			Container.Bind<IPool<GameObject>>().To<GameObjectsPool<ViewFacadesPrefabsProvider, DefaultInstantiationProvider>>().AsSingle().WithArguments(stashGo.transform).NonLazy();
			Container.Bind<IViewFacadeFactory>().To<PoolingViewFacadeFactory>().AsSingle().NonLazy();
			Container.Bind<IViewLogicFactory>().To<ViewLogicFactory>().AsSingle().NonLazy();
			Container.Bind<IMapService>().To<MapService>().AsSingle().NonLazy();
		}
		#endregion

		#region UnityMembers
		public override void Start()
		{
			Initialize().Forget();
		}
		#endregion
	}
}