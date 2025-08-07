using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using Game.Data;

namespace Game.Core
{
    public class AudioEnviromentSystem : MonoBehaviour
    {
        private AudioEnviromentEvaluator evaluator;

        public void Initialize(List<AudioEnvConfig> audiosCurrentDay, int daysPassed, IEventBus<SFXEvent> sfxEventBus)
        {
            evaluator = new AudioEnviromentEvaluator(audiosCurrentDay, sfxEventBus);

            StartCoroutine(CheckAudioConditionsCoroutine(60));
        }

        private IEnumerator CheckAudioConditionsCoroutine(float interval)
        {
            var wait = new WaitForSeconds(interval);

            while (true)
            {
                evaluator.UpdateTime(interval);
                yield return wait;
            }
        }

      
    }
}
