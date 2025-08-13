using NUnit.Framework;
using UnityEngine;

namespace Game.Services.Tests
{
    internal class ServicesFishTankTests
    {
        private FishTankBoundsService testFishTankBoundService;
        private MeshCollider testMesh;

        [SetUp]
        public void SetUp()
        {
            var gameobject = new GameObject();
            testMesh = gameobject.AddComponent<MeshCollider>();
            testFishTankBoundService = new FishTankBoundsService(testMesh);
        }

        [TearDown]
        public void TearDown()
        {
            testMesh = null;
        }

        [Test]
        public void GetMinAndMaxBounds_ShouldReturnCorrectValues()
        {
            Vector2 min = testFishTankBoundService.GetMinBounds();
            Vector2 max = testFishTankBoundService.GetMaxBounds();

            Bounds bounds = testMesh.bounds;
            Vector3 bl = new Vector2(bounds.min.x,bounds.min.y);
            Vector3 tr = new Vector2(bounds.max.x, bounds.max.y);

            Assert.AreEqual(bl.x, min.x, 0.001f);
            Assert.AreEqual(bl.y, min.y, 0.001f);
            Assert.AreEqual(tr.x, max.x, 0.001f);
            Assert.AreEqual(tr.y, max.y, 0.001f);
        }

        [Test]
        public void IsInsideBounds_ShouldReturnTrue_WhenInside()
        {

            Vector2 center = Vector2.zero;
            Assert.IsTrue(testFishTankBoundService.IsInsideBounds(center));
        }
    }
}
