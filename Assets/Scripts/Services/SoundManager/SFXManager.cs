using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
using System.Linq;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public EventBus<SFXEvent> sfxEventBus;
    public AudioEmitterData[] soundEffects;
    private void Awake()
    {
        foreach (AudioEmitterData s in soundEffects)
        {
            s.InstancePrefab = Instantiate(s.AudioSource, this.gameObject.transform);
        }
    }

    private void PlaySFX(AudioEmitterData audio)
    {
        audio.InstancePrefab.Stop();
        audio.InstancePrefab.Play();
    }

    public void onPlaySFXPitched(AudioEmitterData audio)
    {
        var currentAudio = soundEffects.FirstOrDefault(s => s.name == audio.name);
        if (currentAudio != null)
        {
            currentAudio.AudioSource.pitch = Random.Range(.8f, 1.2f);
            PlaySFX(currentAudio);
        }
        else
        {
            Debug.Log("No se encuentra el audio de nombre" + audio.name + " en los recursos cargados.");
        }
    }
}
