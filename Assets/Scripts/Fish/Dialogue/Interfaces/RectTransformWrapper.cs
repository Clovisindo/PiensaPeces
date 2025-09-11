using UnityEngine;

namespace Game.Fishes
{
    public class RectTransformWrapper : IRectTransformWrapper
    {
        private readonly RectTransform _rectTransform;
        public RectTransformWrapper(RectTransform rectTransform) => _rectTransform = rectTransform;
        public Vector2 SizeDelta { get => _rectTransform.sizeDelta; set => _rectTransform.sizeDelta = value; }
    }
}
