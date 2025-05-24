using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
using Assets.Scripts.Services.FoodService;
using System;
using UnityEngine;

public class FoodSpawnerController : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private Transform spawnPoint;
    private FoodManagerService foodManagerService;
    private IEventBus<FoodEaten> foodEatentBus;
    private IEventBus<FoodSpawned> foodSpawnedBus;


    public void Init(FoodManagerService foodManagerService, EventBus<FoodEaten> foodEatentEventBus, EventBus<FoodSpawned> foodSpawnedEventBus)
    {
        this.foodManagerService = foodManagerService;
        this.foodEatentBus = foodEatentEventBus;
        this.foodSpawnedBus = foodSpawnedEventBus;
    }

    public void SpawnFood()
    {
        var food = Instantiate(foodPrefab, spawnPoint.position, Quaternion.identity);
        food.GetComponent<Food>().Init(foodManagerService, foodEatentBus);

        foodSpawnedBus.Raise(new FoodSpawned{food = food});
    }
}
