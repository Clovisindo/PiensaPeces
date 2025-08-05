using Game.Data;
using Game.Events;
using Game.Utilities;
using System.Collections.Generic;

namespace Game.Core
{
    public class AudioEnviromentEvaluator
    {
        private readonly Dictionary<AudioEnvConfig, bool> hasPlayed = new();
        private readonly IEventBus<SFXEvent> eventBus;
        private readonly List<AudioEnvConfig> currentDayAudioConfigs;
        private float elapsedMinutes;

        public AudioEnviromentEvaluator(List<AudioEnvConfig> configs, IEventBus<SFXEvent> eventBus)
        {
            this.currentDayAudioConfigs = configs;
            this.eventBus = eventBus;

            foreach (var config in configs)
                hasPlayed[config] = false;
        }

        public void UpdateTime(float deltaTime)
        {
            elapsedMinutes += deltaTime / 60f;
            EvaluateConditions();
        }

        private void EvaluateConditions()
        {
            foreach (var audioConfig in currentDayAudioConfigs)
            {
                if (hasPlayed[audioConfig]) continue;

                if (ShouldPlayAudio(audioConfig))
                {
                    eventBus.Raise(new SFXEvent { sfxData = audioConfig.AudioEmitterData });
                    hasPlayed[audioConfig] = true;
                }
            }
        }

        private bool ShouldPlayAudio(AudioEnvConfig config)
        {
            return config.triggerCondition switch
            {
                AudioTriggerCondition.TimePassedMinutes => elapsedMinutes >= config.triggerValue,
                _ => false,
            };
        }
    }
}
