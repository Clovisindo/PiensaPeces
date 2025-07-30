namespace Game.Events
{
    public interface IEventBus<T> where T : IEvent
    {
        void Register(EventBinding<T> binding);
        void Deregister(EventBinding<T> binding);
        void Raise(T gameEvent);
    }
}
