using Assets.Scripts.Components;
using Assets.Scripts.Events.Bindings;
using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
using Assets.Scripts.Services.Bounds;
using Assets.Scripts.States;
using UnityEngine;

public class PlayerFishController : BaseFishController
{
    private EventBinding<FoodEaten> foodEatenEventBinding;
    private EventBinding<FoodSpawned> foodSpawnedEventBinding;

    private IBoundsService boundsService;

    private IEventBus<FoodEaten> foodEatentBus;
    private IEventBus<FoodSpawned> foodSpawnedBus;

    private Transform target;

    protected override void Awake()
    {
        base.Awake();
    }

    internal void Init(IBoundsService boundsService, EventBus<FoodEaten> foodEatentEventBus, EventBus<FoodSpawned> foodSpawnedEventBus)
    {
        this.boundsService = boundsService;
        var limiter = GetComponent<TransformLimiter>();
        if (limiter != null)
        {
            limiter.Init(boundsService);
        }

        this.foodEatentBus = foodEatentEventBus;
        this.foodSpawnedBus = foodSpawnedEventBus;

        foodEatentEventBus.Register(new EventBinding<FoodEaten>(OnFoodEaten));
        foodSpawnedEventBus.Register(new EventBinding<FoodSpawned>(OnFoodSpawned ));
    }

    private void OnDisable()
    {
        foodEatentBus?.Deregister(foodEatenEventBinding);
        foodSpawnedBus?.Deregister(foodSpawnedEventBinding);
    }

    private void Start()
    {
        stateMachine.ChangeState(new IdleState(this, stateMachine));
    }

    private void OnFoodSpawned(FoodSpawned foodSpawnedEvent)
    {
        target = foodSpawnedEvent.food.transform;
        stateMachine.ChangeState(new FollowTargetState(this, stateMachine, speed, target));
    }

    private void OnFoodEaten()
    {
        stateMachine.ChangeState(new SwimState(this, boundsService, stateMachine, speed));
    }


}
