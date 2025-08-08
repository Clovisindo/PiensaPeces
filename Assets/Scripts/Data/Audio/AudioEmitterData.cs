using System;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "AudioSFX/AudioSFXData", fileName = "AudioSFXData")]
    public class AudioEmitterData : ScriptableObject
    {
        [SerializeField] String audioName;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioSource instancePrefab;// es necesario instanciar en la escena los prefabs de audioSource para funcionar

        public string AudioName { get => audioName; set => audioName = value; }
        public AudioSource AudioSource { get => audioSource; set => audioSource = value; }
        public AudioSource InstancePrefab { get => instancePrefab; set => instancePrefab = value; }
    }
}