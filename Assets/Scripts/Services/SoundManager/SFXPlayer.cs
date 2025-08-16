using Game.Data;
using Game.Utilities;
using System;
using System.Linq;

namespace Game.Services
{
    public class SFXPlayer : ISFXPlayer
    {
        private readonly Func<float, float, float> randomRange;
        private readonly ILogger logger;

        public SFXPlayer(ILogger logger, Func<float,float,float> randomRange = null)
        {
            this.logger = logger;
            this.randomRange = randomRange ?? UnityEngine.Random.Range;
        }
        public void PlayPitched(AudioEmitterData[] audioEffects, AudioEmitterData requested, float pitchMin, float pitchMax)
        {
            var current = audioEffects.FirstOrDefault(s => s.AudioName == requested.AudioName);
            if (current == null)
            {
                logger.LogWarning("No se encuentra el audio de nombre " +  requested.AudioName);
                return;
            }
            current.AudioSourceWrapper.pitch = randomRange(pitchMin, pitchMax);
            current.InstancePrefabWrapper.Stop();
            current.InstancePrefabWrapper.Play();
        }
    }
}
