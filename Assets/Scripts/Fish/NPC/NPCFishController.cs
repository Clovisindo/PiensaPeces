using Assets.Scripts.Fish.Dialogue;
using Game.Components;
using Game.Core;
using Game.Data;
using Game.StateMachineManager;
using Game.Utilities;
using NSubstitute.Routing.Handlers;
using UnityEngine;

namespace Game.Fishes
{

    public class NPCFishController : BaseFishController
    {
        private NPCFishPool _pool;
        private TransformLimiter _limiter;
        private ExitableFish _exitFishComponent;
        private FishTalker _talker;
        private NPCFishLogic _logic;
        private ITimeService _timeService;

        private NPCFishDependencies _dependencies;

        private bool isInitialized = false;

        public void Init(
           FishConfig config,
           NPCFishPool pool,
           NPCFishDependencies dependencies,
           int daysPassed)
        {
            _pool = pool;
            _dependencies = dependencies;
            _timeService = dependencies.TimeService;

            // --- Renderer ---
            var renderer = GetComponent<SpriteRenderer>();
            if (renderer != null && config.fishSprite != null)
            {
                renderer.sprite = config.fishSprite;
            }

            // --- Limiter ---
            _limiter = GetComponent<TransformLimiter>();
            _limiter?.Init(dependencies.BoundsService);

            // --- Talker ---
            _talker = GetComponent<FishTalker>();
            var depsTalker = new FishTalkerDependencies(
                new NPCFishDialogueEvaluator(),
                new DialoguePathResolver(),
                dependencies.ResourceLoader,
                dependencies.GameObjectFactory,
                dependencies.TimeService,
                dependencies.Global,
                dependencies.SfxEventBus,
                config,
                daysPassed
            );
            
            _talker.Init(depsTalker);

            // --- AI ---
            var ai = new NPCFishAI(dependencies.RandomService.Value);

            // --- Exit ---
            _exitFishComponent = new ExitableFish();
            _exitFishComponent.Init(this, pool);

            // --- State Machine ---
            stateMachine = new StateMachine();
            stateManager = new StateManager(stateMachine);

            // --- Logic ---
            _logic = new NPCFishLogic(
                _dependencies,
                this,
                config,
                _exitFishComponent,
                ai.EvaluateIntent,
                intent => ApplyIntent(intent)
            );

            _logic.StartIntentLoop();
            isInitialized = true;
        }


        protected override void Update()
        {
            if (!isInitialized || _dependencies == null)
            {
                return;
            }
            else
            {
                base.Update();
                _logic.Tick(_timeService.DeltaTime);
            }
        }

        public void NotifyExit()
        {
            ResetFish();
            _pool.RecycleFish(this);
        }

        public void ResetFish()
        {
            _logic.ResetFish(_talker, stateMachine, _limiter);
        }

        private void ApplyIntent(FishIntent intent)
        {
            _logic.ApplyIntent(intent, stateManager, _limiter, transform);
            if (_logic.LifeTimeBehaviour())
            {
                _limiter.enabled = false;
            }
        }

        public float GetLifeTime() => _logic.LifeTime;
    }
}