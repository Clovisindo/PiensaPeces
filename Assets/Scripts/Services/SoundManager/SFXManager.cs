using Assets.Scripts.Events.Bindings;
using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
using System.Linq;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioEmitterData[] soundEffects;

    private IEventBus<SFXEvent> sfxBus;
    private EventBinding<SFXEvent> sfxBinding;
    private void Awake()
    {
        foreach (AudioEmitterData s in soundEffects)
        {
            s.InstancePrefab = Instantiate(s.AudioSource, this.gameObject.transform);
        }
    }

    public void Init(IEventBus<SFXEvent> sfxBus)
    {
        this.sfxBus = sfxBus;
        sfxBinding = new EventBinding<SFXEvent>(OnSFXInvoke);
        sfxBus.Register(sfxBinding);
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
            Debug.LogWarning("No se encuentra el audio de nombre" + audio.name + " en los recursos cargados.");
        }
    }

    private void OnSFXInvoke(SFXEvent e)
    {
        onPlaySFXPitched(e.sfxData);
    }

    private void OnDisable()
    {
        sfxBus?.Deregister(sfxBinding);
    }
}
