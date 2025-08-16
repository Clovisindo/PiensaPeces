namespace Game.Events
{
    public interface IEventBus<T> where T : IEvent
    {
        void Register(IEventBinding<T> binding);
        void Deregister(IEventBinding<T> binding);
        void Raise(T gameEvent);
    }
}
