using System.Collections;
using UnityEngine;

namespace Game.Fishes
{
    public interface ICoroutineRunner
    {
        Coroutine StartDisplayCoroutine(IEnumerator routine);
        void StopCurrentDisplayCoroutine();
        bool HasActiveCoroutine { get; }
    }
}
