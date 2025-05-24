using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
using Assets.Scripts.Services.FoodService;
using UnityEngine;

public class Food : MonoBehaviour
{
    private FoodManagerService foodManager;
    private IEventBus<FoodEaten> foodEatentBus;

    public void Init(FoodManagerService foodManager,  IEventBus<FoodEaten> foodEatentBus)
    {
        this.foodManager = foodManager;
        this.foodEatentBus = foodEatentBus;

        foodManager.RegisterFood(gameObject);
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
