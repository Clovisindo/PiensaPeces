using UnityEngine;

namespace Game.Services
{
    public interface IBoundsService
    {
        Vector2 GetMinBounds();
        Vector2 GetMaxBounds();
        bool IsInsideBounds(Vector2 position);
    }
}
