using Game.Services;
using UnityEngine;

namespace Game.FishLogic
{
    public interface IExitable
    {
        void MoveInDirection(Transform transform, Vector2 direction, float speed);
        bool IsOutOfBounds(Transform transform, IBoundsService boundsService);
        void NotifyExited();
    }
}
