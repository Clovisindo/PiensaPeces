using Game.Services;
using UnityEngine;

namespace Assets.Tests.Data.Mocks
{
    public class MockBoundService : IBoundsService
    {
        public Vector2 GetMaxBounds() { return new Vector2 (0, 0); }

        public Vector2 GetMinBounds() { return new Vector2(0, 0); }

        public bool IsInsideBounds(Vector2 position){ return true; }
    }
}
