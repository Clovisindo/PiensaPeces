using Game.Utilities;
using UnityEngine;


namespace Game.Data
{
    [CreateAssetMenu(fileName = "AudioEnvConfig", menuName = "Audio Env/Audio Config")]
    public class AudioEnvConfig : ScriptableObject
    {
        [SerializeField] AudioEmitterData audioEmitterData;
        public AudioTriggerCondition triggerCondition;
        public float triggerValue;

        public AudioEmitterData AudioEmitterData { get => audioEmitterData; set => audioEmitterData = value; }
        public AudioTriggerCondition TriggerCondition { get => triggerCondition; set => triggerCondition = value; }
        public float TriggerValue { get => triggerValue; set => triggerValue = value; }
    }
}
