using Game.Data;
using UnityEngine;

namespace Game.Events
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
