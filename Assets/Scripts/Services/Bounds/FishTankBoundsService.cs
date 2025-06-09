using UnityEngine;

namespace Assets.Scripts.Services.Bounds
{
    public class FishTankBoundsService : IBoundsService
    {
        private readonly Vector2 minBounds;
        private readonly Vector2 maxBounds;

        public FishTankBoundsService(MeshCollider fishTankCollider)
        {
            UnityEngine.Bounds bounds = fishTankCollider.bounds;

            minBounds = new Vector2(bounds.min.x, bounds.min.y);
            maxBounds = new Vector2(bounds.max.x, bounds.max.y);
        }

        public Vector2 GetMinBounds() => minBounds;
        public Vector2 GetMaxBounds() => maxBounds;

        public bool IsInsideBounds(Vector2 position)
        {
            return position.x >= minBounds.x && position.x <= maxBounds.x &&
                   position.y >= minBounds.y && position.y <= maxBounds.y;
        }
    }
}
