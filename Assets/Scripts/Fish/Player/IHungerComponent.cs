using Game.Events;

namespace Game.Fishes
{
    public interface IHungerComponent
    {
        bool IsHungry { get; }
        void Init(IEventBus<HungryEvent> hungryBus);
        void ResetHunger();
    }
}
