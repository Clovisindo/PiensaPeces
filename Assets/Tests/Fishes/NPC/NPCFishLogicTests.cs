using Game.Components;
using Game.Core;
using Game.Data;
using Game.FishLogic;
using Game.Services;
using Game.StateMachineManager;
using Game.Utilities;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using UnityEngine;

namespace Game.Fishes.Tests
{
    public class NPCFishLogicTests
    {
        NPCFishDependencies dependencies;
        FishConfig config;
        IFish fish;
        IExitable exitable;
        NPCFishLogic fishLogic;
        ICoroutineRunner fishCoroutineRunner;
        IFishStateFactory fishStateFactory;
        IState swimState;
        IState idleState; 
        IState exitState;


        [SetUp]
        public void Setup()
        {
            fishCoroutineRunner = Substitute.For<ICoroutineRunner>();
            fishStateFactory = Substitute.For<IFishStateFactory>();
            swimState = new MockSwimState();
            idleState = new MockIdleState();
            exitState = new MockExitState();

            fishStateFactory.CreateSwimState(Arg.Any<IFish>(),Arg.Any<IBoundsService>(),Arg.Any<float>()).Returns(swimState);
            fishStateFactory.CreateIdleState(Arg.Any<IFish>()).Returns(idleState);
            fishStateFactory.CreateExitState(Arg.Any<Transform>(), Arg.Any<IBoundsService>(), Arg.Any<IExitable>(), Arg.Any<float>()).Returns(exitState);

            dependencies = TestHelpers.CreateDefaultDependencies(coroutineRunner: fishCoroutineRunner, fishStateFactory: fishStateFactory);
            config = TestHelpers.CreateConfig(maxLifetime: 2f);
            fish = Substitute.For<IFish>();
            exitable = Substitute.For<IExitable>();
        }

        [TearDown]
        public void TearDown()
        {
            fishLogic = null;
        }

        [Test]
        public void Tick_IncrementsLifeTime_And_LifeTimeBehaviour_Changes()
        {
            fishLogic = new NPCFishLogic(dependencies, fish, config, exitable, () => FishIntent.Idle, _ => { });

            Assert.IsFalse(fishLogic.LifeTimeBehaviour());

            fishLogic.Tick(1.0f);
            Assert.IsFalse(fishLogic.LifeTimeBehaviour(), "después de 1s no debe expirar.");

            fishLogic.Tick(1.5f);
            Assert.IsTrue(fishLogic.LifeTimeBehaviour(), "después de  más de 2s debería expirar.");
        }

        [Test]
        public void StartIntentLoop_CallsCoroutineRunner_Run()
        {
            fishLogic = new NPCFishLogic(dependencies, fish, config, exitable, () => FishIntent.Idle, _ => { });

            fishLogic.StartIntentLoop();

            fishCoroutineRunner.Received(1).StartDisplayCoroutine(Arg.Any<IEnumerator>());
        }

        [TestCase(FishIntent.SwimRandomly)]
        [TestCase (FishIntent.Idle)]
        public void ApplyIntent_NotExpired_ChangeStateToFactoryState(FishIntent intent)
        {
            fishLogic = new NPCFishLogic(dependencies, fish, config, exitable, () => FishIntent.Idle, _ => { });

            // usamos un SM real para testear la funcionalidad
            var sm = new StateMachine();
            var manager = new StateManager(sm);
            var limiterGO = new GameObject("limiter");
            var limiter = limiterGO.AddComponent<TransformLimiter>();

            fishLogic.ApplyIntent(intent, manager, limiter, new GameObject().transform);

            if (intent == FishIntent.SwimRandomly)
                Assert.AreSame(swimState,sm.currentState);
            else
                Assert.AreSame(idleState, sm.currentState);

            GameObject.DestroyImmediate(limiterGO);
        }

        [Test]
        public void ApplyIntent_WhenExpired_AppliesExitState_And_DisableLimiter()
        {
            fishLogic = new NPCFishLogic(dependencies, fish, config, exitable, () => FishIntent.Idle, _ => { });

            fishLogic.Tick(2.5f);
            Assert.IsTrue(fishLogic.LifeTimeBehaviour());

            // usamos un SM real para testear la funcionalidad
            var sm = new StateMachine();
            var manager = new StateManager(sm);
            var limiterGO = new GameObject("limiter");
            var limiter = limiterGO.AddComponent<TransformLimiter>();

            fishLogic.ApplyIntent(FishIntent.SwimRandomly,manager, limiter, new GameObject().transform);

            Assert.That(sm.currentState, Is.EqualTo(exitState));
            Assert.IsFalse(limiter.enabled, "Se debe desactivar el limiter al salir el pez");

            GameObject.DestroyImmediate(limiterGO);
        }
    }

    static class TestHelpers
    {
        public static NPCFishDependencies CreateDefaultDependencies(
            IFishStateFactory fishStateFactory = null,
            IBoundsService boundsService = null,
            IResourceLoader resourceLoader = null,
            IGlobal global = null,
            IGameObjectFactory gameObjectFactory = null,
            IRandomService randomService = null,
            IYieldInstruction yieldFactory = null,
            ICoroutineRunner coroutineRunner = null,
            ITimeService timeService = null)
        {
            fishStateFactory ??= Substitute.For<IFishStateFactory>();
            boundsService ??= Substitute.For<IBoundsService>();
            resourceLoader ??= Substitute.For<IResourceLoader>();
            global ??= Substitute.For<IGlobal>();
            gameObjectFactory ??= Substitute.For<IGameObjectFactory>();
            randomService ??= Substitute.For<IRandomService>();
            yieldFactory ??= Substitute.For<IYieldInstruction>();
            coroutineRunner ??= Substitute.For<ICoroutineRunner>();
            timeService ??= Substitute.For<ITimeService>();

            return new NPCFishDependencies(
                monoBehaviour: null,
                fishStateFactory : fishStateFactory,
                boundsService: boundsService,
                resourceLoader: resourceLoader,
                global: global,
                sfxEventBus: null, // no hace falta para estos tests
                gameObjectFactory: gameObjectFactory,
                randomService: randomService,
                yieldFactory: yieldFactory,
                coroutineRunner: coroutineRunner,
                timeService: timeService
            );
        }

        public static FishConfig CreateConfig(float maxLifetime = 10f, float speed = 1f)
        {
            var cfg = ScriptableObject.CreateInstance<FishConfig>();
            cfg.maxLifetime = maxLifetime;
            cfg.speed = speed;
            return cfg;
        }
    }

    public class MockIdleState : IState
    {
        public void Enter() { }
        public void Exit() { }
        public void Update() { }
    }
    public class MockSwimState : IState
    {
        public void Enter() { }
        public void Exit() { }
        public void Update() { }
    }
    public class MockExitState : IState
    {
        public void Enter() { }
        public void Exit() { }
        public void Update() { }
    }
}
