using System.Collections;

namespace Game.Fishes
{
    public interface ICoroutineRunner
    {
        void StartDisplayCoroutine(IEnumerator routine);
        void StopCurrentDisplayCoroutine();
        bool HasActiveCoroutine { get; }
    }
}
