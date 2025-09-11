using UnityEngine;

namespace Game.Fishes
{ 
    public class UnityCoroutineHandle : ICoroutineHandle
    {
        private readonly Coroutine _coroutine;
        public UnityCoroutineHandle(Coroutine coroutine) => _coroutine = coroutine;
        public bool IsValid => _coroutine != null;
        public Coroutine UnityCoroutine => _coroutine;
    }
}
