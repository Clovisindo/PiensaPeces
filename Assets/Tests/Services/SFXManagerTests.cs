using Game.Data;
using NSubstitute;
using NUnit.Framework;
using System;
using UnityEngine;
using ILogger = Game.Utilities.ILogger;

namespace Game.Services.Tests
{
    public class SFXManagerTests
    {
        private SFXPlayer sfxPlayer;
        private ILogger loggerMock;
        private IAudioSourceWrapper audioSourceMock;
        private IAudioSourceWrapper instancePrefabMock;
        private AudioEmitterData audioEmitterData;
        private AudioEmitterData emptyAudioEmmitterData;
        private AudioEmitterData[] soundEffects;

        const string fishAudioName = "fishTalk";
        const string emptyAudioName = "test";

        [SetUp]
        public void Setup()
        {
            SetupPlayerSFX();
            SetupSoundEffects();
        }

        [TearDown]
        public void Teardown()
        {
            DestroyEmitter(audioEmitterData);
            DestroyEmitter(emptyAudioEmmitterData);
        }

        [Test]
        public void PlayCurrentAudioWithRandomPitch()
        {
            sfxPlayer.PlayPitched(soundEffects, audioEmitterData, 0.8f, 1.2f);

            Assert.AreEqual(1.0f, audioSourceMock.pitch, "El pitch deberia ser el valor fijo que asignamos con el random.");
            Received.InOrder(() =>
            {
                instancePrefabMock.Stop();
                instancePrefabMock.Play();
            });
            loggerMock.DidNotReceiveWithAnyArgs().LogWarning(default);
        }

        [Test]
        public void DontPlayWhenAudioDontExist()
        {
            sfxPlayer.PlayPitched(soundEffects, emptyAudioEmmitterData, 0.8f, 1.2f);

            instancePrefabMock.Received(0).Stop();
            instancePrefabMock.Received(0).Play();
            loggerMock.Received(1).LogWarning("No se encuentra el audio de nombre " + emptyAudioName);

        }

        [Test]
        public void PlayPitched_AlwaysReturnsSamePitch_WhenRandomInjectedIsDeterministic()
        {
            sfxPlayer.PlayPitched(soundEffects, audioEmitterData, 0.8f, 1.2f);

            Assert.AreEqual(1.0f, audioSourceMock.pitch, "Con random inyectado determinista, el pitch siempre debe ser 1.0f");
        }

        [Test]
        public void PlayPitched_UsesUnityRandomRange_WhenNoRandomInjected()
        {
            // creamos un player sin random inyectado
            sfxPlayer = new SFXPlayer(loggerMock);

            sfxPlayer.PlayPitched(soundEffects, audioEmitterData, 0.8f, 1.2f);

            Assert.GreaterOrEqual(audioSourceMock.pitch, 0.8f, "El pitch debe ser mayor o igual al mínimo.");
            Assert.LessOrEqual(audioSourceMock.pitch, 1.2f, "El pitch debe ser menor o igual al máximo.");
        }

        private void SetupSoundEffects()
        {
            audioSourceMock = Substitute.For<IAudioSourceWrapper>();
            instancePrefabMock = Substitute.For<IAudioSourceWrapper>();
            audioEmitterData = ScriptableObject.CreateInstance<AudioEmitterData>();
            audioEmitterData.AudioName = fishAudioName;
            audioEmitterData.AudioSourceWrapper = audioSourceMock;
            audioEmitterData.Source = new GameObject().AddComponent<AudioSource>();
            audioEmitterData.InstancePrefab = new GameObject().AddComponent<AudioSource>();
            audioEmitterData.InstancePrefabWrapper = instancePrefabMock;
            soundEffects = new[] { audioEmitterData };

            emptyAudioEmmitterData = ScriptableObject.CreateInstance<AudioEmitterData>();
            emptyAudioEmmitterData.AudioName = emptyAudioName;
            emptyAudioEmmitterData.AudioSourceWrapper = audioSourceMock;
            emptyAudioEmmitterData.Source = new GameObject().AddComponent<AudioSource>();
            emptyAudioEmmitterData.InstancePrefab = new GameObject().AddComponent<AudioSource>();
            emptyAudioEmmitterData.InstancePrefabWrapper = instancePrefabMock;
        }

        private void SetupPlayerSFX()
        {
            loggerMock = Substitute.For<ILogger>();
            Func<float, float, float> fakeRandom = (min, max) => 1.0f;
            sfxPlayer = new SFXPlayer(loggerMock, fakeRandom);
        }

        private void DestroyEmitter(AudioEmitterData emitterData)
        {
            if (emitterData == null) return;
            if (emitterData.Source != null)
                GameObject.DestroyImmediate(emitterData.Source.gameObject);
            if (emitterData.InstancePrefab != null)
                GameObject.DestroyImmediate(emitterData.InstancePrefab.gameObject);
        }
    }
}
