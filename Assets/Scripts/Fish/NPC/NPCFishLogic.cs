using Game.Components;
using Game.Core;
using Game.Data;
using Game.Events;
using Game.FishLogic;
using Game.Services;
using Game.StateMachineManager;
using Game.Utilities;
using System;
using UnityEngine;

namespace Game.Fishes
{
    public class NPCFishLogic
    {
        private readonly NPCFishDependencies _dependencies;
        private readonly IFish _fish;
        private readonly FishConfig _config;
        private readonly IFishIntentScheduler _intentScheduler;
        private readonly IFishStateFactory _fishStateFactory;
        private readonly IBoundsService _boundsService;
        private readonly IExitable _exitFishComponent;

        private float _lifeTime;
        public float LifeTime => _lifeTime;
        private readonly float _maxLifeTime;
        private readonly float _speed;

        public NPCFishLogic(NPCFishDependencies dependencies,
            IFish fish,
            FishConfig config,
            IExitable exitableFish,
            Func<FishIntent> evaluateIntent,
            Action<FishIntent> applyIntent)
        {
            _dependencies = dependencies;
            _fish = fish;
            _config = config;
            _exitFishComponent = exitableFish;

            _maxLifeTime = config.maxLifetime;
            _speed = config.speed;

            _intentScheduler = new NPCFishIntentScheduler(dependencies.CoroutineRunner, dependencies.YieldFactory, _config, evaluateIntent, applyIntent);
            _fishStateFactory = _dependencies.FishStateFactory;
            _boundsService = _dependencies.BoundsService;
        }

        public void StartIntentLoop() => _intentScheduler.StartEvaluatingPeriodically();
        public void StopIntentLoop() => _intentScheduler.Stop();

        public void Tick(float deltatime) => _lifeTime += deltatime;

        public bool LifeTimeBehaviour() => _lifeTime > _maxLifeTime;

        public void ResetFish(FishTalker talker, IStateMachine stateMachine, TransformLimiter limiter)
        {
            _lifeTime = 0;
            limiter.enabled = true;
            talker.ResetTalker();
            stateMachine.ChangeState(_fishStateFactory.CreateSwimState(_fish, _boundsService, _speed));
        }

        public void ApplyIntent(FishIntent intent, StateManager stateManager, TransformLimiter limiter, Transform transform)
        {
            if (LifeTimeBehaviour())
            {
                stateManager.ApplyState(_fishStateFactory.CreateExitState(transform, _boundsService, _exitFishComponent, _speed));
                _intentScheduler.Stop();
                limiter.enabled = false;
            }
            else
            {
                switch (intent)
                {
                    case FishIntent.SwimRandomly:
                        stateManager.ApplyState(_fishStateFactory.CreateSwimState(_fish, _boundsService, _speed));
                        break;
                    default:
                        stateManager.ApplyState(_fishStateFactory.CreateIdleState(_fish));
                        break;
                }
            }
        }

    }

    public class NPCFishDependencies
    {
        public IFishStateFactory FishStateFactory { get;  }
        public IBoundsService BoundsService { get;  }
        public IResourceLoader ResourceLoader { get; }
        public IGlobal Global { get; }
        public IGameObjectFactory GameObjectFactory { get; }
        public IRandomService RandomService { get; }
        public IYieldInstruction YieldFactory { get; }
        public ICoroutineRunner CoroutineRunner { get; }
        public ITimeService TimeService { get; }
        public EventBus<SFXEvent> SfxEventBus { get;  }


        public NPCFishDependencies(
            MonoBehaviour monoBehaviour,
            IFishStateFactory fishStateFactory,
            IBoundsService boundsService,
            IResourceLoader resourceLoader,
            IGlobal global,
            EventBus<SFXEvent> sfxEventBus,
            IGameObjectFactory gameObjectFactory = null,
            IRandomService randomService = null,
            IYieldInstruction yieldFactory = null,
            ICoroutineRunner coroutineRunner = null,
            ITimeService timeService = null)
        {
            FishStateFactory = fishStateFactory;
            BoundsService = boundsService;
            FishStateFactory = fishStateFactory;
            ResourceLoader = resourceLoader;
            Global = global;
            SfxEventBus = sfxEventBus;

            GameObjectFactory = gameObjectFactory ?? new UnityGameObjectFactory();
            TimeService = timeService ?? new UnityTimeService();
            CoroutineRunner = coroutineRunner ?? new UnityCoroutineRunner(monoBehaviour);
            RandomService = randomService ?? new UnityRandomService();
            YieldFactory = yieldFactory ?? new UnityYieldInstruction();

        }
    }
}
