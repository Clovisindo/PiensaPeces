using Assets.Scripts.Events.Bindings;
using Assets.Scripts.Events.Events;

namespace Assets.Scripts.Events.EventBus
{
    public interface IEventBus<T> where T : IEvent
    {
        void Register(EventBinding<T> binding);
        void Deregister(EventBinding<T> binding);
        void Raise(T gameEvent);
    }
}
