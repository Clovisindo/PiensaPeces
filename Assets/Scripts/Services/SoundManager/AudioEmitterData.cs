using System;
using UnityEngine;


[CreateAssetMenu(menuName = "AudioSFX/AudioSFXData", fileName = "AudioSFXData")]
public class AudioEmitterData : ScriptableObject
{
    [SerializeField] String AudioName;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource instancePrefab;// es necesario instanciar en la escena los prefabs de audioSource para funcionar

    public string AudioName1 { get => AudioName; set => AudioName = value; }
    public AudioSource AudioSource { get => audioSource; set => audioSource = value; }
    public AudioSource InstancePrefab { get => instancePrefab; set => instancePrefab = value; }
}
