using UnityEngine;

namespace Game.Data
{
    public class UnityAudioSourceWrapper : IAudioSourceWrapper
    {
        private readonly AudioSource source;
        public UnityAudioSourceWrapper(AudioSource source) => this.source = source;
        public AudioSource audioSource { get => source; }
        public float pitch { get => source.pitch; set => source.pitch = value; }
        public void Play() => source.Play();
        public void Stop() => source.Stop();
        
    }
}
