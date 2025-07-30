using Game.Services;
using UnityEngine;

namespace Game.Fishes
{ 
    public interface IExitableFish
    {
        void MoveInDirection(Transform transform, Vector2 direction, float speed);
        bool IsOutOfBounds(Transform transform,IBoundsService boundsService);
        void NotifyExited();
    }
}
