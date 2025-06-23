using Assets.Scripts.Events.Bindings;
using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
using Assets.Scripts.Services.Enviroment;
using Assets.Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class AudioEnviromentSystem : MonoBehaviour
    {
        private List<AudioEnvConfig> currentDayAudioConfigs;
        private Dictionary<AudioEnvConfig, bool> hasPlayed = new();
        private IEventBus<SFXEvent> eventBus;

        private float elapsedTime = 0f;
        private int fishFedCount = 0;

        public void Initialize(List<AudioEnvConfig> audiosCurrentDay, int daysPassed, IEventBus<SFXEvent> sfxEventBus)
        {
            this.eventBus = sfxEventBus;

            currentDayAudioConfigs = audiosCurrentDay;
            hasPlayed.Clear();

            foreach (var c in currentDayAudioConfigs)
                hasPlayed[c] = false;

            StartCoroutine(CheckAudioConditionsCoroutine(60));
        }

        public void OnFishFed() => fishFedCount++;// ToDo: no implementado


        public void Update()
        {
            elapsedTime += Time.deltaTime;
        }
        private IEnumerator CheckAudioConditionsCoroutine(float interval)
        {
            var wait = new WaitForSeconds(interval);

            while (true)
            {
                EvaluateConditions();
                yield return wait;
            }
        }

        private void EvaluateConditions()
        {
           

            foreach (var audioConfig in currentDayAudioConfigs)
            {
                if (hasPlayed[audioConfig]) continue;

                if (ShouldPlayAudio(audioConfig))
                {
                    eventBus.Raise(new SFXEvent { sfxData = audioConfig.AudioEmitterData});
                    hasPlayed[audioConfig] = true;
                }
            }
        }
        private bool ShouldPlayAudio(AudioEnvConfig config)
        {
            switch (config.triggerCondition)
            {
                case AudioTriggerCondition.TimePassedMinutes:
                    return elapsedTime / 60f >= config.triggerValue;

                case AudioTriggerCondition.FishFedTimes:
                    return fishFedCount >= config.triggerValue;

                default:
                    return false;
            }
        }
    }
}
