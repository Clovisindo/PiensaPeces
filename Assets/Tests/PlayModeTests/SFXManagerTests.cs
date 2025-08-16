using Game.Data;
using Game.Events;
using Game.Services;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using ILogger = Game.Utilities.ILogger;


namespace Assets.Tests.PlayModeTests
{
    public class SFXManagerTests
    {
        private SFXManager sfxManager;
        private AudioEmitterData emitterData;
        private AudioSource prefabAudioSource;
        private IEventBus<SFXEvent> sfxBus;
        private ISFXPlayer sfxPlayer;
        private EventBinding<SFXEvent> capturedBinding;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            var prefabGO = new GameObject("PrefabAudioSource");
            prefabAudioSource = prefabGO.AddComponent<AudioSource>();

            emitterData = ScriptableObject.CreateInstance<AudioEmitterData>();
            emitterData.AudioName = "Test";
            emitterData.Source = prefabAudioSource;
            emitterData.InstancePrefab = null;

            var go = new GameObject("SFXManager");
            sfxManager = go.AddComponent<SFXManager>();
            sfxManager.soundEffects = new[] {emitterData};

            sfxBus = Substitute.For<IEventBus<SFXEvent>>();
            sfxBus.When(x => x.Register(Arg.Any<IEventBinding<SFXEvent>>()))
                .Do(ci => { capturedBinding = ci.Arg<IEventBinding<SFXEvent>>() as EventBinding<SFXEvent>;});
            sfxPlayer = Substitute.For<ISFXPlayer>();
            sfxManager.Init(sfxPlayer, sfxBus);
            sfxManager.InitSoundEffects();

            yield return null;
        }


        [UnityTest]
        public IEnumerator WhenAwake_InstanceAllPrefabs()
        {
            
            Assert.IsNotNull(emitterData.InstancePrefab, "InstancePrefab debería haberse instanciado en Awake");
            Assert.AreEqual(sfxManager.transform, emitterData.InstancePrefab.transform.parent, "Debe ser hijo del SFXManager");

            yield break;
        }


        [UnityTest]
        public IEnumerator WhenInit_RegisterEventBus()
        {
            
            sfxBus.Received(1).Register(Arg.Any<EventBinding<SFXEvent>>());
            yield break;
        }

        [UnityTest]
        public IEnumerator WhenInvokeEvent_ThenReproduceAudio()
        {
            CaptureEventBindingFromBus(sfxBus);

            var sfxEvent = new SFXEvent() { sfxData = emitterData };
            InvokeSfxEvent(sfxEvent);

            sfxPlayer.Received(1).PlayPitched(
                sfxManager.soundEffects,
                emitterData,
                Arg.Any<float>(),
                Arg.Any<float>()
                );
            yield break;
        }

        [UnityTest]
        public IEnumerator WhenAudioDontExist_DontPlayAnything()
        {
            var loggerMock = Substitute.For<ILogger>();
            sfxPlayer = new SFXPlayer(loggerMock, (min, max) => 1.0f);
            CaptureEventBindingFromBus(sfxBus);
            sfxManager.Init(sfxPlayer, sfxBus);


            var emptyEmitterData = ScriptableObject.CreateInstance<AudioEmitterData>();
            emptyEmitterData.AudioName = "TestNoAudio";

            InvokeSfxEvent(new SFXEvent { sfxData = emptyEmitterData });

            loggerMock.Received(1).LogWarning("No se encuentra el audio de nombre " + emptyEmitterData.AudioName);
            yield break;
        }

        private void CaptureEventBindingFromBus(IEventBus<SFXEvent> bus)
        {
            bus.When(x => x.Register(Arg.Any<IEventBinding<SFXEvent>>()))
               .Do(ci => { capturedBinding = ci.Arg<IEventBinding<SFXEvent>>() as EventBinding<SFXEvent>; });
        }

        private void InvokeSfxEvent(SFXEvent evt)
        {
            Assert.NotNull(capturedBinding, "No se capturó el EventBinding");
            ((IEventBinding<SFXEvent>)capturedBinding).OnEvent?.Invoke(evt);
        }
    }
}
