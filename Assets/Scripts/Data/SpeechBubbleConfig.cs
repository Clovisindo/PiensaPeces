using UnityEngine;

namespace Game.Data
{
    public struct SpeechBubbleConfig
    {
        public float FontSize;
        public float DisplayDuration;
        public float FadeDuration;
        public Vector2 Padding;

        public static SpeechBubbleConfig Default => new SpeechBubbleConfig
        {
            FontSize = 180f, // 36f * 5
            DisplayDuration = 2f,
            FadeDuration = 0.5f,
            Padding = new Vector2(20f, 20f)
        };
    }
}
