using Game.Components;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests
{
    public class TransformLimiterTests
    {
        private Vector2 minBounds;
        private Vector2 maxBounds;
        private float halfWidth;
        private float halfHeight;


        [SetUp]
        public void Setup()
        {
            minBounds = new Vector2(0, 0);
            maxBounds = new Vector2(10, 10);
            halfWidth = 1;
            halfHeight = 1;
        }

        [TestCase(5, 5, 5, 5)] // Dentro de límites, no se clampa
        [TestCase(-5, 5, 1, 5)] // X por debajo
        [TestCase(15, 5, 9, 5)] // X por encima
        [TestCase(5, -5, 5, 1)] // Y por debajo
        [TestCase(5, 15, 5, 9)] // Y por encima
        [TestCase(-10, -10, 1, 1)] // Ambos por debajo
        [TestCase(20, 20, 9, 9)] // Ambos por encima
        [TestCase(-5, 15, 1, 9)] // X por debajo, Y por encima
        [TestCase(15, -5, 9, 1)] // X por encima, Y por debajo
        [Test]
        public void Position_ShouldBeClampedWihtinBounds(float inputX, float inputY, float expectedX, float expectedY)
        {
            var input = new Vector2(inputX, inputY);
            var expected = new Vector2(expectedX, expectedY);

            var result = TransformLimiterLogic.ClampPosition(input, minBounds, maxBounds, halfWidth, halfHeight);

            Assert.AreEqual(expected, result);
        }
    }
}
