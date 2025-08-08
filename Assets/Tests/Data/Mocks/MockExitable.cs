using Game.FishLogic;
using Game.Services;
using UnityEngine;

namespace Assets.Tests.Data.Mocks
{
    internal class MockExitable : IExitable
    {
        public bool IsOutOfBounds(Transform transform, IBoundsService boundsService){ return true; }

        public void MoveInDirection(Transform transform, Vector2 direction, float speed) { }

        public void NotifyExited() { }
    }
}
