using UnityEngine;
using Game.Services;

namespace Game.Components
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TransformLimiter : MonoBehaviour
    {
        private IBoundsService boundsService;
        private float halfWidth;
        private float halfHeight;

        public void Init(IBoundsService service)
        {
            boundsService = service;

            var sr = GetComponent<SpriteRenderer>();
            halfWidth = sr.bounds.size.x / 2f;
            halfHeight = sr.bounds.size.y / 2f;
        }

        private void LateUpdate()
        {
            if (boundsService == null) return;

            var pos = transform.position;

            var clampPosition = TransformLimiterLogic.ClampPosition(
                pos,
                boundsService.GetMinBounds(),
                boundsService.GetMaxBounds(),
                halfWidth,
                halfHeight);

            transform.position = clampPosition;
        }
    }
}
