using Assets.Scripts.Fish.Dialogue;
using Game.Components;
using Game.Core;
using Game.Data;
using Game.Events;
using Game.FishLogic;
using Game.Services;
using Game.StateMachineManager;
using Game.Utilities;
using UnityEngine;

namespace Game.Fishes
{

    public class NPCFishController : BaseFishController
    {
        private NPCFishPool pool;
        private IBoundsService boundsService;
        private TransformLimiter limiter;
        private NPCFishAI ai;
        private IFishIntentScheduler intentScheduler;
        private ExitableFish exitFishComponent;
        private FishTalker talker;
        private IFishStateFactory FishStateFactory;

        private float lifeTime;
        private float maxLifeTime;
        public void Init(FishConfig config, NPCFishPool pool, IFishStateFactory fishFactory, IBoundsService boundsService, int daysPassed, EventBus<SFXEvent> sfxEventBus)
        {
            this.pool = pool;
            this.boundsService = boundsService;
            this.FishStateFactory = fishFactory;

            var renderer = GetComponent<SpriteRenderer>();
            if (renderer != null && config.fishSprite != null)
            {
                renderer.sprite = config.fishSprite;
            }

            limiter = GetComponent<TransformLimiter>();
            limiter?.Init(boundsService);

            talker = GetComponent<FishTalker>();
            var dependenciesFishTalker = new FishTalkerDependencies(
                new NPCFishDialogueEvaluator(),
                new DialoguePathResolver(),
                new UnityResourceLoader(),
                new UnityGameObjectFactory(),
                new UnityTimeService(),
                new UnityGlobal(),
                 sfxEventBus, config, daysPassed);
            this.talker.Init(dependenciesFishTalker);

            ai = new NPCFishAI(Random.value);
            exitFishComponent = new ExitableFish();
            exitFishComponent.Init(this, pool);
            intentScheduler = new NPCFishIntentScheduler(this, config, ai.EvaluateIntent, ApplyIntent);
            this.maxLifeTime = config.maxLifetime;
            this.speed = config.speed;

            stateMachine = new StateMachine();
            stateManager = new StateManager(stateMachine);
            intentScheduler.StartEvaluatingPeriodically();
        }

        protected override void Update()
        {
            base.Update();
            lifeTime += Time.deltaTime;
        }

        public void NotifyExit()
        {
            ResetFish();
            pool.RecycleFish(this);
        }

        public void ResetFish()
        {
            lifeTime = 0;
            limiter.enabled = true;
            talker.ResetTalker();
            stateMachine.ChangeState(FishStateFactory.CreateSwimState(this, boundsService, speed));
        }

        private bool LifeTimeBehaviour()
        {
            if (lifeTime > maxLifeTime)
            {
                return true;
            }
            return false;
        }

        private void ApplyIntent(FishIntent intent)
        {
            if (LifeTimeBehaviour())
            {
                //var exitContext = new ExitScreenContext(transform, boundsService, this.exitFishComponent, speed);
                //stateManager.ApplyState(new ExitScreenState(exitContext));
                stateManager.ApplyState(FishStateFactory.CreateExitState(transform, boundsService, this.exitFishComponent, speed));
                intentScheduler.Stop();
                limiter.enabled = false;
            }
            else
            {
                switch (intent)
                {
                    case FishIntent.SwimRandomly:
                        //stateManager.ApplyState(new SwimState(this, boundsService, speed));
                        stateManager.ApplyState(FishStateFactory.CreateSwimState(this,boundsService,speed));
                        break;
                    case FishIntent.Idle:
                    default:
                        //stateManager.ApplyState(new IdleState(this));
                        stateManager.ApplyState(FishStateFactory.CreateIdleState(this));
                        break;
                }
            }
        }
    }
}