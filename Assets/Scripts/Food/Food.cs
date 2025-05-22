using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
using UnityEngine;

public class Food : MonoBehaviour
{
    //[SerializeField] private FoodEatenEventChannelSO foodEatenEvent;

    private IEventBus<FoodEaten> foodEatentBus;


    public void Init(IEventBus<FoodEaten> foodEatentBus)
    {
        this.foodEatentBus = foodEatentBus;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colisión con: " + other.name);
        if (other.CompareTag("PlayerFish"))
        {
            foodEatentBus.Raise(new FoodEaten());
            //foodEatenEvent.Raise();
            Destroy(gameObject);
        }
    }
}
