using UnityEngine;
using Assets.Scripts.Services.Bounds;

namespace Assets.Scripts.Components
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
            var min = boundsService.GetMinBounds();
            var max = boundsService.GetMaxBounds();

            pos.x = Mathf.Clamp(pos.x, min.x + halfWidth, max.x - halfWidth);
            pos.y = Mathf.Clamp(pos.y, min.y + halfHeight, max.y - halfHeight);

            transform.position = pos;
        }
    }
}
