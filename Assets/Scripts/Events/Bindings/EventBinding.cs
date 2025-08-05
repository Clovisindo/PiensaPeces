using System;

namespace Game.Events
{
    internal interface IEventBinding<T>
    {
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }
    }

    public class EventBinding<T> : IEventBinding<T> where T : IEvent
    {
        Action<T> onEvent = _ => { };
        Action OnEventNoArgs = () => { };
        Action<T> IEventBinding<T>.OnEvent
        {
            get => onEvent;
            set => onEvent = value;
        }
        Action IEventBinding<T>.OnEventNoArgs
        {
            get => OnEventNoArgs;
            set => OnEventNoArgs = value;
        }


        public EventBinding(Action<T> onEvent) => this.onEvent = onEvent;
        public EventBinding(Action onEventNoArgs) => this.OnEventNoArgs = onEventNoArgs;

        public void Add(Action onEvent) => OnEventNoArgs += onEvent;
        public void Remove(Action onEvent) => OnEventNoArgs -= onEvent;

        public void Add(Action<T> onEvent) => this.onEvent += onEvent;
        public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;
    }
}
