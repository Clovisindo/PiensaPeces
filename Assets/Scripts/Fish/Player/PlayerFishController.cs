using Game.Events;
using System;
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
        //temporal
        [SerializeField] FishConfig config;

        public void Init(FishConfig playerConfig, IBoundsService boundsService, FoodManagerService foodManagerService, SFXManager sFXManager, int daysPassed,
            EventBus<FoodEaten> foodEatentEventBus, EventBus<FoodSpawned> foodSpawnedEventBus, EventBus<HungryEvent> hungryEventBus, EventBus<SFXEvent> sfxEventBus)
        {
            this.config = playerConfig;
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
            stateManager.ApplyState(new IdleState(this, stateMachine));

            this.boundsService = boundsService;
            limiter.Init(boundsService);
            this.hungerComponent.Init(hungryEventBus);
            this.talker.Init(new PlayerFishDialogueEvaluator(hungerComponent), playerConfig, sfxEventBus, daysPassed);
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
                    stateManager.ApplyState(new SwimState(this, boundsService, stateMachine, speed));
                    break;
                case FishIntent.FollowFood:
                    var target = ai.GetTargetFood();
                    if (target != null)
                        stateManager.ApplyState(new FollowTargetState(this, stateMachine, speed, target));
                    break;
                case FishIntent.Idle:
                default:
                    stateManager.ApplyState(new IdleState(this, stateMachine));
                    break;
            }
        }
    }
}