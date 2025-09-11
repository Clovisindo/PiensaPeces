using UnityEngine;

namespace Game.Fishes
{
    public class CanvasGroupWrapper : ICanvasGroupWrapper
    {
        private readonly CanvasGroup _canvasGroup;
        public CanvasGroupWrapper(CanvasGroup canvasGroup) => _canvasGroup = canvasGroup;
        public float Alpha { get => _canvasGroup.alpha; set => _canvasGroup.alpha = value; }
    }
}
