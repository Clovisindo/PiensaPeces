using Assets.Scripts.Utilities;
using System;
using UnityEngine;

namespace Assets.Scripts.Services.Enviroment
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
