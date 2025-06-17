using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
using Assets.Scripts.Services.Bounds;
using Assets.Scripts.Services.FoodService;
using System;
using UnityEngine;

public class FoodSpawnerController : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] float foodFallSpeed = 1.0f;
    private IBoundsService boundsService;
    private FoodManagerService foodManagerService;
    private IEventBus<FoodEaten> foodEatentBus;
    private IEventBus<FoodSpawned> foodSpawnedBus;


    public void Init(IBoundsService boundsService, FoodManagerService foodManagerService, EventBus<FoodEaten> foodEatentEventBus, EventBus<FoodSpawned> foodSpawnedEventBus)
    {
        this.boundsService = boundsService;
        this.foodManagerService = foodManagerService;
        this.foodEatentBus = foodEatentEventBus;
        this.foodSpawnedBus = foodSpawnedEventBus;
    }

    public void SpawnFood()
    {
        var min = boundsService.GetMinBounds();
        var max = boundsService.GetMaxBounds();

        float randomX = UnityEngine.Random.Range(min.x, max.x);
        Vector2 spawnPosition = new Vector2(randomX, max.y);

        var food = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
        food.GetComponent<Food>().Init(foodManagerService, foodFallSpeed, min.y, foodEatentBus);

        foodSpawnedBus.Raise(new FoodSpawned{food = food});
    }
}
