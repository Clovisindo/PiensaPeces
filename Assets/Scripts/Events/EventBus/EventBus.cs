using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public class EventBus<T> : IEventBus<T> where T : IEvent
    {
        private readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();

        public void Register(IEventBinding<T> binding) => bindings.Add(binding);
        public void Deregister(IEventBinding<T> binding) => bindings.Remove(binding);
        public void Raise(T gameEvent)
        {
            foreach (var binding in bindings)
            {
                binding.OnEvent.Invoke(gameEvent);
                binding.OnEventNoArgs.Invoke();
            }
        }

        public void Clear()
        {
            Debug.Log($"Clearing {typeof(T).Name} bindings");
            bindings.Clear();
        }
    }
}
