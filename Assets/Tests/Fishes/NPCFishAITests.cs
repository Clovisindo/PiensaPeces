using Game.Utilities;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Game.Fishes.Tests
{
    public class NPCFishAITests
    {
        private IRandomService _randomService;
        private IMathClamp _mathClamp;
        private const float SWIM_PROBABILITY = 0.5f;



        [SetUp]
        public void Setup()
        {
            _randomService = Substitute.For<IRandomService>();
            _mathClamp = Substitute.For<IMathClamp>();
        }

        [Test]
        public void NPCFishAI_InitCorrectly()
        {
            float? currentSwimProb = null;
            _mathClamp.When(x => x.MathfClamp(Arg.Any<float>()))
                .Do(x => currentSwimProb = x.Arg<float>());
            
            var _npcFishAI = new NPCFishAI(SWIM_PROBABILITY, _mathClamp, _randomService);

            _mathClamp.Received(1).MathfClamp(SWIM_PROBABILITY);
            _randomService.Received(0);
            Assert.That(currentSwimProb, Is.InRange(0f, 1f));
        }

        [TestCase(-1f)]
        [TestCase(0f)]
        [TestCase(1f)]
        [TestCase(2f)]
        public void NPCFishAI_CallsClamp_And_AlwaysGetsValueBetween0And1(float input)
        {
            float clampedValue = -99f;

            var fakeMathClamp = Substitute.For<IMathClamp>();
            fakeMathClamp.MathfClamp(Arg.Any<float>())
                .Returns(ci =>
                {
                    var v = ci.Arg<float>();
                    clampedValue = Mathf.Clamp01(v); // calculamos el real
                    return clampedValue;
                });

            var random = Substitute.For<IRandomService>();

            var _npcFishAI = new NPCFishAI(input, fakeMathClamp, random);

            Assert.That(clampedValue, Is.InRange(0f, 1f));
        }

        [TestCase(0.5f, 0.3f, FishIntent.SwimRandomly, TestName = "ValueBelowThreshold_ShouldSwim")]
        [TestCase(0.5f, 0.5f, FishIntent.Idle, TestName = "ValueEqualThreshold_ShouldIdle")]
        [TestCase(0.5f, 0.8f, FishIntent.Idle, TestName = "ValueAboveThreshold_ShouldIdle")]
        public void EvaluateIntent_ReturnsExpectedIntent(float swimProbability, float randomValue,
            FishIntent expectedIntent)
        {
            _mathClamp.MathfClamp(Arg.Any<float>()).Returns(swimProbability);
            _randomService.Value.Returns(randomValue);
            var ai = new NPCFishAI(swimProbability, _mathClamp, _randomService);

            var result = ai.EvaluateIntent();

            Assert.That(result, Is.EqualTo(expectedIntent));
        }

        [Test]
        public void EvaluateIntent_ShouldCallRandomService()
        {
            var ai = new NPCFishAI(SWIM_PROBABILITY, _mathClamp, _randomService);

            var result = ai.EvaluateIntent();

            _ = _randomService.Received(1).Value;
        }
    }
}
