using UnityEngine;

namespace Game.Services
{
    public class CameraBoundsService : IBoundsService
    {
        private readonly Vector2 minBounds;
        private readonly Vector2 maxBounds;

        public CameraBoundsService(Camera cam)
        {
            var bl = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
            var tr = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

            minBounds = new Vector2(bl.x, bl.y);
            maxBounds = new Vector2(tr.x, tr.y);
        }

        public Vector2 GetMinBounds() => minBounds;
        public Vector2 GetMaxBounds() => maxBounds;

        public bool IsInsideBounds(Vector2 position)
        {
            throw new System.NotImplementedException();
        }
    }
}
