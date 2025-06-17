using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
using Assets.Scripts.Food;
using Assets.Scripts.Services.FoodService;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] FoodFallBehaviour foodFallBehaviour;
    
    private FoodManagerService foodManager;
    private IEventBus<FoodEaten> foodEatentBus;

    public void Init(FoodManagerService foodManager, float foodFallSpeed, float minY,  IEventBus<FoodEaten> foodEatentBus)
    {
        this.foodManager = foodManager;
        this.foodEatentBus = foodEatentBus;

        foodManager.RegisterFood(gameObject);
        foodFallBehaviour.Init(foodFallSpeed, minY);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colisión con: " + other.name);
        if (other.CompareTag("PlayerFish"))
        {
            foodEatentBus.Raise(new FoodEaten());
            foodManager.UnregisterFood(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        foodManager?.UnregisterFood(gameObject); 
    }
}
