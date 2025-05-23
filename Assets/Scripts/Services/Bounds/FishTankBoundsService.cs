using UnityEngine;

namespace Assets.Scripts.Services.Bounds
{
    public class FishTankBoundsService : IBoundsService
    {
        private readonly Vector2 minBounds;
        private readonly Vector2 maxBounds;

        public FishTankBoundsService(Collider2D fishTankCollider)
        {
            UnityEngine.Bounds bounds = fishTankCollider.bounds;

            minBounds = new Vector2(bounds.min.x, bounds.min.y);
            maxBounds = new Vector2(bounds.max.x, bounds.max.y);
        }

        public Vector2 GetMinBounds() => minBounds;
        public Vector2 GetMaxBounds() => maxBounds;
    }
}
