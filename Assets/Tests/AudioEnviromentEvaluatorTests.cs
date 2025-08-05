using Game.Core;
using Game.Data;
using Game.Events;
using Game.Utilities;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tests
{
    public class AudioEnviromentEvaluatorTests
    {
        private IEventBus<SFXEvent> mockBus;
        private AudioEmitterData audioEmitterData;
        private AudioEnvConfig AudioEnvConfig;
        private AudioEnviromentEvaluator evaluator;

        private IEventBus<SFXEvent> CreateMockBus() => Substitute.For<IEventBus<SFXEvent>>();
        private AudioEmitterData CreateAudioData()
        {
            return ScriptableObject.CreateInstance<AudioEmitterData>();
        }
        private AudioEnvConfig CreateTimeBasedConfig(AudioEmitterData data, float minutes)
        {
            return ScriptableObjectTestFactory.CreateWithInit<AudioEnvConfig>(config =>
            {
                config.triggerCondition = AudioTriggerCondition.TimePassedMinutes;
                config.triggerValue = minutes;
                config.AudioEmitterData = data;
            });
        }
        private AudioEnviromentEvaluator CreateEvaluator(AudioEnvConfig config, IEventBus<SFXEvent> bus)
        {
            return new AudioEnviromentEvaluator(new List<AudioEnvConfig> { config }, bus);
        }


        [SetUp]
        public void Setup()
        {
            mockBus = CreateMockBus();
            audioEmitterData = CreateAudioData();
            AudioEnvConfig = CreateTimeBasedConfig(audioEmitterData, 1);
            evaluator = CreateEvaluator(AudioEnvConfig, mockBus);
        }

        [Test]
        public void ShouldTriggerAudio_WhenTimePasses()
        {
            //act
            evaluator.UpdateTime(60);
            //assert
            mockBus.Received(1).Raise(Arg.Is<SFXEvent>(e => e.sfxData == audioEmitterData));
        }

        [Test]
        public void ShouldTriggerOnce_WhenConditionAlreadyMet()
        {
            //act
            evaluator.UpdateTime(60);
            evaluator.UpdateTime(60);
            //assert
            mockBus.Received(1).Raise(Arg.Is<SFXEvent>(e => e.sfxData == audioEmitterData));
        }
    }
}