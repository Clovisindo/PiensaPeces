using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
using System;
using UnityEngine;

public class FoodSpawnerController : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private Transform spawnPoint;
    private IEventBus<FoodEaten> foodEatentBus;
    private IEventBus<FoodSpawned> foodSpawnedBus;


    public void Init(EventBus<FoodEaten> foodEatentEventBus, EventBus<FoodSpawned> foodSpawnedEventBus)
    {
        this.foodEatentBus = foodEatentEventBus;
        this.foodSpawnedBus = foodSpawnedEventBus;
    }

    public void SpawnFood()
    {
        var food = Instantiate(foodPrefab, spawnPoint.position, Quaternion.identity);
        food.GetComponent<Food>().Init(foodEatentBus);

        foodSpawnedBus.Raise(new FoodSpawned
        {
            food = food
        });
    }


}
