using Assets.Scripts.Components;
using Assets.Scripts.Core;
using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
using Assets.Scripts.Fish.Dialogue;
using Assets.Scripts.Fish.Player;
using Assets.Scripts.Services.Bounds;
using Assets.Scripts.Services.FoodService;
using Assets.Scripts.States;

public class PlayerFishController : BaseFishController
{
    private IBoundsService boundsService;
    private HungerComponent hungerComponent;
    private TransformLimiter limiter;
    private PlayerFishAI ai;
    private PlayerFishEventHandler eventHandler;
    private FishIntentScheduler intentScheduler;
    private FishTalker talker;

    protected override void Awake()
    {
        base.Awake();
       
    }

    internal void Init(IBoundsService boundsService, FoodManagerService foodManagerService, EventBus<FoodEaten> foodEatentEventBus,
        EventBus<FoodSpawned> foodSpawnedEventBus, EventBus<HungryEvent> hungryEventBus)
    {
        limiter = GetComponent<TransformLimiter>();
        hungerComponent = GetComponent<HungerComponent>();
        talker = GetComponent<FishTalker>();

        this.boundsService = boundsService;
        if (limiter != null)
        {
            limiter.Init(boundsService);
        }
        this.hungerComponent.Init(hungryEventBus);
        this.talker.Init(hungerComponent);
        ai = new PlayerFishAI(transform, hungerComponent, foodManagerService);
        intentScheduler = new FishIntentScheduler(this, ai.EvaluateIntent, ApplyIntent);
        eventHandler = new PlayerFishEventHandler(intentScheduler, hungerComponent, foodEatentEventBus, foodSpawnedEventBus, hungryEventBus);
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
