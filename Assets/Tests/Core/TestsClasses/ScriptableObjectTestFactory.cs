using UnityEngine;

namespace Game.Core.Tests
{
    public static class ScriptableObjectTestFactory
    {
        public static T CreateWithInit<T>(System.Action<T> init) where T : ScriptableObject
        {
            var instance = ScriptableObject.CreateInstance<T>();
            init?.Invoke(instance);
            return instance;
        }
    }
}
