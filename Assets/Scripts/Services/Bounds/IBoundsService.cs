using UnityEngine;

namespace Assets.Scripts.Services.Bounds
{
    public interface IBoundsService
    {
        Vector2 GetMinBounds();
        Vector2 GetMaxBounds();
    }
}
