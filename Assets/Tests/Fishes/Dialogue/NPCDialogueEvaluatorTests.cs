using Game.Utilities;
using NSubstitute;
using NUnit.Framework;

namespace Game.Fishes.Tests
{
    public class NPCDialogueEvaluatorTests
    {
        private NPCFishDialogueEvaluator evaluator;
        private ITimeService timeService;
        private IRandomService randomService;

        private const string ALWAYS_CONDITION = "Always";
        private const string RANDOM_CONDITION = "Random";

        [SetUp]
        public void Setup()
        {
            timeService = Substitute.For<ITimeService>();
            randomService = Substitute.For<IRandomService>();

            evaluator = new NPCFishDialogueEvaluator(timeService, randomService);
        }

        [TearDown]
        public void TearDown()
        {
            evaluator = null;
            timeService = null;
            randomService = null;
        }

        [Test]
        public void Evaluate_WhenConditionIsAlways_ShouldReturnedTrue()
        {
            bool result = evaluator.Evaluate(ALWAYS_CONDITION);

            Assert.IsTrue(result);
        }

        [Test]
        public void Evaluate_WhenConditionIsNull_ShouldReturnedTrue()
        {
            bool result = evaluator.Evaluate(null);

            Assert.IsTrue(result);
        }

        [Test]
        public void Evaluate_WhenConditionIsEmpty_ShouldReturnTrue()
        {
            bool result = evaluator.Evaluate("");

            Assert.IsTrue(result);
        }

        [Test]
        public void Evaluate_WhenConditionIsWhitespace_ShouldReturnTrue()
        {
            // Act
            bool result = evaluator.Evaluate("   ");

            // Assert
            Assert.IsTrue(result);
        }

        [TestCase(10f, 5f, true)]
        [TestCase(5f, 10f, false)]
        [TestCase(10f, 10f, false)]
        public void Evaluate_TimeCondition_ShouldReturnCorrectResult(float currentTime, float conditionTime, bool expectedResult)
        {
            timeService.Time.Returns(currentTime);
            string condition = $"Time > {conditionTime}";

            bool result = evaluator.Evaluate(condition);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Evaluate_TimeConditionWithSpaces_ShouldParseCorrectly()
        {
            timeService.Time.Returns(15f);
            string condition = $"Time >   10 ";

            bool result = evaluator.Evaluate(condition);

            Assert.IsTrue(result);
        }

        [Test]
        public void Evaluate_TimeConditionWithInvalidNumber_ShouldReturnFalse()
        {
            timeService.Time.Returns(10f);
            string condition = "Time > invalid";

            bool result = evaluator.Evaluate(condition);

            Assert.IsFalse(result);
        }

        [Test]
        public void Evaluate_TimeConditionWithNegativeValue_ShouldWorkCorrectly()
        {
            timeService.Time.Returns(0f);
            string condition = "Time > -5";

            bool result = evaluator.Evaluate(condition);

            Assert.IsTrue(result);
        }

        [TestCase(0.05f, true)]  // 5% < 10%
        [TestCase(0.15f, false)] // 15% > 10%
        [TestCase(0.1f, false)]  // 10% = 10%
        public void Evaluate_RandomCondition_ShouldReturnCorrectResult(float randomValue, bool expected)
        {
            randomService.Value.Returns(randomValue);

            bool result = evaluator.Evaluate(RANDOM_CONDITION);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Evaluate_UnknownCondition_ShouldReturnFalse()
        {
            bool result = evaluator.Evaluate("UnknownCondition");

            Assert.IsFalse(result);
        }

        [Test]
        public void CurrentTime_ShouldReturnTimeProviderValue()
        {
            float expectedTime = 42.5f;
            timeService.Time.Returns(expectedTime);

            float actualTime = evaluator.currentTime;

            Assert.AreEqual(expectedTime, actualTime);
            timeService.Received(1);
        }
    }
}
