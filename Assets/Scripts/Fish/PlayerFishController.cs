using Assets.Scripts.Components;
using Assets.Scripts.Events.Bindings;
using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
using Assets.Scripts.Services.Bounds;
using Assets.Scripts.Services.FoodService;
using Assets.Scripts.States;
using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;

public class PlayerFishController : BaseFishController
{
    private IBoundsService boundsService;
    private HungerComponent hungerComponent;
    private FoodManagerService foodManagerService;

    private EventBinding<FoodEaten> foodEatenEventBinding;
    private EventBinding<FoodSpawned> foodSpawnedEventBinding;
    private EventBinding<HungryEvent> hungryEventBinding;
    private IEventBus<FoodEaten> foodEatentBus;
    private IEventBus<FoodSpawned> foodSpawnedBus;
    private IEventBus<HungryEvent> hungryBus;

    private Coroutine foodCheckRoutine;

    protected override void Awake()
    {
        base.Awake();
    }

    internal void Init(IBoundsService boundsService, FoodManagerService foodManagerService, EventBus<FoodEaten> foodEatentEventBus,
        EventBus<FoodSpawned> foodSpawnedEventBus, EventBus<HungryEvent> hungryEventBus)
    {
        this.boundsService = boundsService;
        this.foodManagerService = foodManagerService;
        var limiter = GetComponent<TransformLimiter>();
        if (limiter != null)
        {
            limiter.Init(boundsService);
        }
        hungerComponent = GetComponent<HungerComponent>();
        

        this.foodEatentBus = foodEatentEventBus;
        this.foodSpawnedBus = foodSpawnedEventBus;
        this.hungryBus = hungryEventBus;

        foodEatenEventBinding = new EventBinding<FoodEaten>(OnFoodEaten);
        foodSpawnedEventBinding = new EventBinding<FoodSpawned>(OnFoodSpawned);
        hungryEventBinding = new EventBinding<HungryEvent>(OnHungry);

        foodEatentEventBus.Register(foodEatenEventBinding);
        foodSpawnedEventBus.Register(foodSpawnedEventBinding);
        hungryEventBus.Register(hungryEventBinding);

        hungerComponent.Init(hungryBus);
    }

    private void OnDisable()
    {
        foodEatentBus?.Deregister(foodEatenEventBinding);
        foodSpawnedBus?.Deregister(foodSpawnedEventBinding);
        hungryBus?.Deregister(hungryEventBinding);
    }

    private void Start()
    {
        stateMachine.ChangeState(new IdleState(this, stateMachine));
    }

    private void OnFoodSpawned(FoodSpawned foodSpawnedEvent)
    {
        if (!hungerComponent.IsHungry || !foodManagerService.HasAnyFood()) return;

        var closestFood = foodManagerService.GetClosestFood(transform.position);
        if (closestFood != null)
        {
            stateMachine.ChangeState(new FollowTargetState(this, stateMachine, speed, closestFood.transform));
        }
        
    }

    private void OnFoodEaten()
    {
        if (foodCheckRoutine != null)
        {
            StopCoroutine(foodCheckRoutine);
            foodCheckRoutine = null;
        }

        hungerComponent.ResetHunger();
        stateMachine.ChangeState(new SwimState(this, boundsService, stateMachine, speed));
    }

    private void OnHungry()
    {
        if (foodCheckRoutine == null)
            foodCheckRoutine = StartCoroutine(CheckForFoodPeriodically());
    }

    private IEnumerator CheckForFoodPeriodically()
    {
        while (hungerComponent.IsHungry)
        {
            var closestFood = foodManagerService.GetClosestFood(transform.position);
            if (closestFood != null)
            {
                stateMachine.ChangeState(new FollowTargetState(this, stateMachine, speed, closestFood.transform));
                foodCheckRoutine = null;
                yield break; 
            }
            yield return new WaitForSeconds(0.5f);
        }
        foodCheckRoutine = null; // limpieza por si se sale por otra vía
    }

    private void OnAllFoodEaten()
    {
        stateMachine.ChangeState(new SwimState(this, boundsService, stateMachine, speed));
    }


}
