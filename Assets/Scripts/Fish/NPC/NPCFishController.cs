using Game.Components;
using Game.Events;
using Game.Services;
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

        private float lifeTime;
        private float maxLifeTime;
        public void Init(FishConfig config, NPCFishPool pool, IBoundsService boundsService, int daysPassed, EventBus<SFXEvent> sfxEventBus)
        {
            this.pool = pool;
            this.boundsService = boundsService;

            var renderer = GetComponent<SpriteRenderer>();
            if (renderer != null && config.fishSprite != null)
            {
                renderer.sprite = config.fishSprite;
            }

            limiter = GetComponent<TransformLimiter>();
            limiter?.Init(boundsService);

            talker = GetComponent<FishTalker>();
            this.talker.Init(new NPCFishDialogueEvaluator(), config, sfxEventBus, daysPassed);

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
            stateMachine.ChangeState(new SwimState(this, boundsService, stateMachine, speed));
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
                var exitContext = new ExitScreenContext(transform, boundsService, exitFishComponent, speed);
                stateManager.ApplyState(new ExitScreenState(exitContext));
                intentScheduler.Stop();
                limiter.enabled = false;
            }
            else
            {
                switch (intent)
                {
                    case FishIntent.SwimRandomly:
                        stateManager.ApplyState(new SwimState(this, boundsService, stateMachine, speed));
                        break;
                    case FishIntent.Idle:
                    default:
                        stateManager.ApplyState(new IdleState(this, stateMachine));
                        break;
                }
            }
        }
    }
}