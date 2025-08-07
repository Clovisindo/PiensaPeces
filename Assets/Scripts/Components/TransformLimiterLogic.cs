using UnityEngine;


namespace Game.Components
{
    public class TransformLimiterLogic
    {
        public static Vector2 ClampPosition(Vector2 position, Vector2 minBounds, Vector2 maxBounds, float halfWidth, float halfHeight)
        {
            return new Vector2(
                Mathf.Clamp(position.x,minBounds.x + halfWidth, maxBounds.x - halfWidth),
                Mathf.Clamp(position.y,minBounds.y + halfHeight, maxBounds.y - halfHeight)
                );
        }
    }
}
