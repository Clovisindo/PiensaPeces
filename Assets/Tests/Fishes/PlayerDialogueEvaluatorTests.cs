using Game.Utilities;
using NSubstitute;
using NUnit.Framework;

namespace Game.Fishes.Tests
{
    public class PlayerDialogueEvaluatorTests
    {
        private PlayerFishDialogueEvaluator evaluator;
        private ITimeService timeService;
        private IRandomService randomService;
        private IHungerComponent hungerComponent;

        private const string ALWAYS_CONDITION = "Always";
        private const string HUNGRY_CONDITION = "IsHungry";
        private const string RANDOM_CONDITION = "Random";

        [SetUp]
        public void Setup()
        {
            timeService = Substitute.For<ITimeService>();
            randomService = Substitute.For<IRandomService>();
            hungerComponent = Substitute.For<IHungerComponent>();
            

            evaluator = new PlayerFishDialogueEvaluator( hungerComponent, timeService, randomService);
        }

        [TearDown]
        public void TearDown()
        {
            evaluator = null;
            timeService = null;
            randomService = null;
        }

        [Test]
        public void Evaluate_WhenConditionIsAlways_ShouldReturnTrue()
        {
            bool result = evaluator.Evaluate(ALWAYS_CONDITION);
            Assert.IsTrue(result);
        }

        [Test]
        public void Evaluate_WhenConditionIsNull_ShouldReturnTrue()
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
            bool result = evaluator.Evaluate("   ");

            Assert.IsTrue(result);
        }

        [TestCase(10f, 5f, true)]
        [TestCase(5f, 10f, false)]
        [TestCase(10f, 10f, false)]
        public void Evaluate_TimeCondition_ShouldReturnCorrectValue(float currentTime, float conditionTime, bool expectedResult)
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

        [TestCase(true, true)]
        [TestCase(false, false)]
        public void Evaluate_HungerComponent_ShouldReturnCorrectValue(bool isHungryStatus,bool expectedResult)
        {
            hungerComponent.IsHungry.Returns(isHungryStatus);
            bool result = evaluator.Evaluate(HUNGRY_CONDITION);
            Assert.AreEqual( expectedResult,result);
        }


        [TestCase(0.05f, true)]  // 5% < 10%
        [TestCase(0.15f, false)] // 15% > 10%
        [TestCase(0.1f, false)]  // 10% = 10%
        public void Evaluate_Random_ShouldReturnCorrectValue(float randomValue, bool expectedResult)
        {
            randomService.Value.Returns(randomValue);

            bool result = evaluator.Evaluate(RANDOM_CONDITION);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Evaluate_UnknownCondition_ShouldReturnFalse()
        {
            bool result = evaluator.Evaluate("unknownCondition");
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
