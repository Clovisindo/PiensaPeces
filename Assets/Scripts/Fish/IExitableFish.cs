using Assets.Scripts.Core;
using Assets.Scripts.Services.Bounds;
using UnityEngine;

namespace Assets.Scripts.Fish
{
    public interface IExitableFish
    {
        void MoveInDirection(Transform transform, Vector2 direction, float speed);
        bool IsOutOfBounds(Transform transform,IBoundsService boundsService);
        void NotifyExited();
    }
}
