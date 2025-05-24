using Assets.Scripts.Components;
using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
using Assets.Scripts.Fish.Player;
using Assets.Scripts.Services.Bounds;
using Assets.Scripts.Services.FoodService;
using Assets.Scripts.States;

public class PlayerFishController : BaseFishController
{
    private IBoundsService boundsService;
    private HungerComponent hungerComponent;
    private FoodManagerService foodManagerService;
    private PlayerFishAI ai;
    private PlayerFishEventHandler eventHandler;

    protected override void Awake()
    {
        base.Awake();
    }

    internal void Init(IBoundsService boundsService, FoodManagerService foodManagerService, EventBus<FoodEaten> foodEatentEventBus,
        EventBus<FoodSpawned> foodSpawnedEventBus, EventBus<HungryEvent> hungryEventBus)
    {
        this.boundsService = boundsService;
        
        var limiter = GetComponent<TransformLimiter>();
        if (limiter != null)
        {
            limiter.Init(boundsService);
        }
        this.foodManagerService = foodManagerService;
        hungerComponent = GetComponent<HungerComponent>();
        hungerComponent.Init(hungryEventBus);
        ai = new PlayerFishAI(transform, hungerComponent, foodManagerService); 
        eventHandler = new PlayerFishEventHandler(this, foodEatentEventBus, foodSpawnedEventBus, hungryEventBus);
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
  
    public void HandleFoodEatenEvent()
    {
        hungerComponent.ResetHunger();
        ApplyIntent(ai.EvaluateIntent());
    }

    public void HandleFoodSpawnedEvent()
    {
        if (!hungerComponent.IsHungry || !foodManagerService.HasAnyFood()) return;
        ApplyIntent(ai.EvaluateIntent());
    }

    public void HandleHungryEvent()
    {
        ApplyIntent(ai.EvaluateIntent());
    }

    private void ApplyIntent(FishIntent intent)
    {
        switch (intent)
        {
            case FishIntent.SwimRandomly:
                stateMachine.ChangeState(new SwimState(this, boundsService, stateMachine, speed));
                break;
            case FishIntent.FollowFood:
                var target = ai.GetTargetFood();
                if (target != null)
                    stateMachine.ChangeState(new FollowTargetState(this, stateMachine, speed, target));
                break;
            case FishIntent.Idle:
            default:
                stateMachine.ChangeState(new IdleState(this, stateMachine));
                break;
        }
    }

}
