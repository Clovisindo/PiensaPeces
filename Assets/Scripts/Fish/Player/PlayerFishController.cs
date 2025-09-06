using Assets.Scripts.Fish.Dialogue;
using Assets.Scripts.Services.TimeService;
using Game.Components;
using Game.Core;
using Game.Data;
using Game.Events;
using Game.FishFood;
using Game.FishLogic;
using Game.Services;
using Game.StateMachineManager;
using Game.Utilities;
using UnityEngine;

namespace Game.Fishes
{
    public class PlayerFishController : BaseFishController
    {
        private IBoundsService boundsService;
        private HungerComponent hungerComponent;
        private TransformLimiter limiter;
        private PlayerFishAI ai;
        private PlayerFishEventHandler eventHandler;
        private IFishIntentScheduler intentScheduler;
        private FishTalker talker;
        private IFishStateFactory FishStateFactory;
        //temporal
        [SerializeField] FishConfig config;

        public void Init(FishConfig playerConfig, IFishStateFactory fishFactory, IBoundsService boundsService, FoodManagerService foodManagerService, SFXManager sFXManager, int daysPassed,
            EventBus<FoodEaten> foodEatentEventBus, EventBus<FoodSpawned> foodSpawnedEventBus, EventBus<HungryEvent> hungryEventBus, EventBus<SFXEvent> sfxEventBus)
        {
            this.config = playerConfig;
            this.FishStateFactory = fishFactory;
            limiter = GetComponent<TransformLimiter>();
            hungerComponent = GetComponent<HungerComponent>();
            talker = GetComponent<FishTalker>();

            var renderer = GetComponent<SpriteRenderer>();
            if (renderer != null && config.fishSprite != null)
            {
                renderer.sprite = config.fishSprite;
            }

            stateMachine = new StateMachine();
            stateManager = new StateManager(stateMachine);
            stateManager.ApplyState(FishStateFactory.CreateIdleState(this));

            this.boundsService = boundsService;
            limiter.Init(boundsService);
            this.hungerComponent.Init(hungryEventBus);
            var dependenciesFishTalker = new FishTalkerDependencies(
                new PlayerFishDialogueEvaluator(hungerComponent),
                new DialoguePathResolver(),
                new UnityResourceLoader(),
                new UnityGameObjectFactory(),
                new UnityTimeService(),
                new UnityGlobal(),
                 sfxEventBus, config, daysPassed);
            this.talker.Init(dependenciesFishTalker);
            ai = new PlayerFishAI(transform, hungerComponent, foodManagerService);
            intentScheduler = new PlayerFishIntentScheduler(this, config, ai.EvaluateIntent, ApplyIntent);
            eventHandler = new PlayerFishEventHandler(intentScheduler, hungerComponent, sFXManager, foodEatentEventBus, foodSpawnedEventBus, hungryEventBus);
            eventHandler.RegisterEvents();
        }

        private void OnDisable()
        {
            eventHandler?.DeregisterEvents();
        }

        private void Start()
        {
            ApplyIntent(FishIntent.Idle);
        }

        private void ApplyIntent(FishIntent intent)
        {
            switch (intent)
            {
                case FishIntent.SwimRandomly:
                    stateManager.ApplyState(FishStateFactory.CreateSwimState(this, boundsService, speed));
                    break;
                case FishIntent.FollowFood:
                    var target = ai.GetTargetFood();
                    if (target != null)
                        stateManager.ApplyState(FishStateFactory.CreateFollowState(this, speed, target));
                    break;
                case FishIntent.Idle:
                default:
                    stateManager.ApplyState(FishStateFactory.CreateIdleState(this));
                    break;
            }
        }
    }
}