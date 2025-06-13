using UnityEngine;

namespace Assets.Scripts.Events.Events
{
    public interface IEvent{ }
    public struct FoodEaten : IEvent { }
    public struct FoodSpawned : IEvent
    {
        public GameObject food;
    }
    public struct HungryEvent: IEvent { }

    public struct SFXEvent : IEvent
    {
        public AudioEmitterData sfxData;
    }
}
