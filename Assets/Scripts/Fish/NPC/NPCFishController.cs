using Assets.Scripts.Components;
using Assets.Scripts.Core;
using Assets.Scripts.Fish;
using Assets.Scripts.Fish.Dialogue;
using Assets.Scripts.Fish.NPC;
using Assets.Scripts.Fish.Player;
using Assets.Scripts.Services.Bounds;
using Assets.Scripts.States;
using Assets.Scripts.Utilities;
using UnityEngine;

public class NPCFishController : BaseFishController
{
    private NPCFishPool pool;
    private IBoundsService boundsService;
    private TransformLimiter limiter;
    private NPCFishAI ai;
    private PlayerFishIntentScheduler intentScheduler;
    private ExitableFish exitFishComponent;
    private FishTalker talker;

    private float lifeTime;
    private float maxLifeTime;
    public void Init(FishConfig config, NPCFishPool pool, IBoundsService boundsService)
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
        this.talker.Init(new NPCFishDialogueEvaluator());

        ai = new NPCFishAI(UnityEngine.Random.value);
        exitFishComponent = new ExitableFish();
        exitFishComponent.Init(this, pool);
        intentScheduler = new PlayerFishIntentScheduler(this, ai.EvaluateIntent, ApplyIntent);
        this.maxLifeTime = config.maxLifetime;
        this.speed = config.speed;

        stateMachine = new StateMachine();
        stateManager = new StateManager(stateMachine);
        intentScheduler.StartEvaluatingPeriodically(10f);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void NotifyExit()
    {
        pool.RecycleFish(this);
    }

    public void ResetFish()
    {
        lifeTime = 0;
        limiter.enabled = true;
        stateMachine.ChangeState(new SwimState(this, boundsService, stateMachine, speed));
    }

    private bool LifeTimeBehaviour()
    {
        lifeTime += Time.deltaTime;
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