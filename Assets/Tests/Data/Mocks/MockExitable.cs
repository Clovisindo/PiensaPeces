using Game.FishLogic;
using Game.Services;
using UnityEngine;

namespace Game.Data.Tests
{
    internal class MockExitable : IExitable
    {
        public bool IsOutOfBounds(Transform transform, IBoundsService boundsService){ return true; }

        public void MoveInDirection(Transform transform, Vector2 direction, float speed) { }

        public void NotifyExited() { }
    }
}
