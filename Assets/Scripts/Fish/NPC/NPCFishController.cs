using Assets.Scripts.Components;
using Assets.Scripts.Fish.Dialogue;
using Assets.Scripts.Fish.NPC;
using Assets.Scripts.Fish.Player;
using Assets.Scripts.Services.Bounds;
using Assets.Scripts.States;

public class NPCFishController : BaseFishController
{
    private IBoundsService boundsService;
    private TransformLimiter limiter;
    private NPCFishAI ai;
    private PlayerFishIntentScheduler intentScheduler;
    private FishTalker talker;
    public void Init(IBoundsService boundsService)
    {
        limiter = GetComponent<TransformLimiter>();
        limiter?.Init(boundsService);
        this.boundsService = boundsService;
        talker = GetComponent<FishTalker>();
        this.talker.Init(new NPCFishDialogueEvaluator());
        ai = new NPCFishAI(UnityEngine.Random.value);
        intentScheduler = new PlayerFishIntentScheduler(this, ai.EvaluateIntent, ApplyIntent);

        stateMachine.ChangeState(new SwimState(this, boundsService, stateMachine, speed));
    }

    protected override void Update()
    {
        base.Update();
        intentScheduler.StartEvaluatingPeriodically(3f);
    }

    private void ApplyIntent(FishIntent intent)
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