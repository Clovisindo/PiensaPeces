using Game.Data;
using Game.Events;
using UnityEngine;

namespace Game.Services
{
    public class SFXManager : MonoBehaviour
    {
        public AudioEmitterData[] soundEffects;
        private ISFXPlayer player;
        private IEventBus<SFXEvent> sfxBus;
        private EventBinding<SFXEvent> sfxBinding;

        private void Awake()
        {
            if (soundEffects == null) { return; }
            InitSoundEffects();
        }

        public void InitSoundEffects()
        {
            foreach (AudioEmitterData s in soundEffects)
            {
                s.AudioSourceWrapper = new UnityAudioSourceWrapper(s.Source);
                s.InstancePrefab = Instantiate(s.AudioSourceWrapper.audioSource, this.gameObject.transform);
                s.InstancePrefabWrapper = new UnityAudioSourceWrapper(s.InstancePrefab);
            }
        }

        public void Init(ISFXPlayer player, IEventBus<SFXEvent> sfxBus)
        {
            this.player = player;
            this.sfxBus = sfxBus;
            sfxBinding = new EventBinding<SFXEvent>(OnSFXInvoke);
            sfxBus.Register(sfxBinding);
        }

        private void OnSFXInvoke(SFXEvent e)
        {
            player.PlayPitched(soundEffects,e.sfxData, 0.8f, 1.2f);
        }

        private void OnDisable()
        {
            sfxBus?.Deregister(sfxBinding);
        }
    }
}