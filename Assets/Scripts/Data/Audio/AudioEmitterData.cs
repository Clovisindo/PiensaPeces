using System;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "AudioSFX/AudioSFXData", fileName = "AudioSFXData")]
    public class AudioEmitterData : ScriptableObject
    {
        [SerializeField] String audioName;
        [SerializeField] IAudioSourceWrapper audioSourceWrapper;//wraper para extraer funcionalidad de unity
        [SerializeField] AudioSource source;// asignacion prefab desde editor
        [SerializeField] IAudioSourceWrapper instancePrefabWrapper;//wraper para extraer funcionalidad de unity
        [SerializeField] AudioSource instancePrefab;// es necesario instanciar en la escena los prefabs de audioSource para funcionar

        public string AudioName { get => audioName; set => audioName = value; }
        public IAudioSourceWrapper AudioSourceWrapper { get => audioSourceWrapper; set => audioSourceWrapper = value; }
        public AudioSource Source { get => source; set => source = value; }
        public IAudioSourceWrapper InstancePrefabWrapper { get => instancePrefabWrapper; set => instancePrefabWrapper = value; }
        public AudioSource InstancePrefab { get => instancePrefab; set => instancePrefab = value; }
    }
}