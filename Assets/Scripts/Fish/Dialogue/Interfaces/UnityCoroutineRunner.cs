using System.Collections;
using UnityEngine;

namespace Game.Fishes
{
    public class UnityCoroutineRunner : ICoroutineRunner
    {
        private readonly MonoBehaviour _monoBehaviour;
        private Coroutine _currentCoroutine;

        public UnityCoroutineRunner(MonoBehaviour monoBehaviour) => _monoBehaviour = monoBehaviour;

        public bool HasActiveCoroutine => _currentCoroutine != null;

        public Coroutine StartDisplayCoroutine(IEnumerator routine)
        {
            StopCurrentDisplayCoroutine();
            return _currentCoroutine = _monoBehaviour.StartCoroutine(routine);
        }

        public void StopCurrentDisplayCoroutine()
        {
            if (_currentCoroutine != null)
            {
                _monoBehaviour.StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }
        }
    }
}
