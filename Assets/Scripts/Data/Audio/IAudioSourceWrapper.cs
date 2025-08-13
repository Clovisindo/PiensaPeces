using UnityEngine;

namespace Game.Data
{
    public interface IAudioSourceWrapper
    {
        float pitch {  get; set; }
        AudioSource audioSource { get; }
        void Stop();
        void Play();
    }
}
