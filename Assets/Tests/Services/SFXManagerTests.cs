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
        private SFXPlayer player;
        private ILogger loggerMock;

        [SetUp]
        public void Setup()
        {
            loggerMock = Substitute.For<ILogger>();
            Func<float, float, float> fakeRandom = (min, max) => 1.0f;
            player = new SFXPlayer(loggerMock, fakeRandom);
        }

        [Test]
        public void PlayCurrentAudioWithRandomPitch()
        {
            var audioSourceMock = Substitute.For<AudioSource>();
            var audioEmitterData = ScriptableObject.CreateInstance<AudioEmitterData>();
            audioEmitterData.name = "fishTalk";
            //audioEmitterData.AudioSource = audioSourceMock;
            var soundEffects = new[] { audioEmitterData };
            

            player.PlayPitched(soundEffects, audioEmitterData, 0.8f, 1.2f);

            Assert.AreEqual(1.0f, audioSourceMock.pitch, "El pitch deberia ser el valor fijo que asignamos con el random.");
            Received.InOrder(() =>
            {
                audioSourceMock.Stop();
                audioSourceMock.Play();
            });
            loggerMock.DidNotReceiveWithAnyArgs().LogWarning(default);
        }
    }
}
