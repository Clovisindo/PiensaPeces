using Game.Services;
using NUnit.Framework;
using UnityEngine;

namespace Game.Services.Tests
{
    public class ServicesCameraTests
    {
        private Camera testCamera;

        [SetUp]
        public void SetUp()
        {
            var camGO = new GameObject("TestCamera");
            testCamera = camGO.AddComponent<Camera>();
            testCamera.orthographic = true;
            testCamera.orthographicSize = 5f;
            testCamera.transform.position = new Vector3(0, 0, -10);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(testCamera.gameObject);
        }

        [Test]
        public void GetMinAndMaxBounds_ShouldReturnCorrectValues()
        {
            var boundsService = new CameraBoundsService(testCamera);

            Vector2 min = boundsService.GetMinBounds();
            Vector2 max = boundsService.GetMaxBounds();

            Vector3 bl = testCamera.ViewportToWorldPoint(new Vector3(0, 0, testCamera.nearClipPlane));
            Vector3 tr = testCamera.ViewportToWorldPoint(new Vector3(1, 1, testCamera.nearClipPlane));

            Assert.AreEqual(bl.x, min.x, 0.001f);
            Assert.AreEqual(bl.y, min.y, 0.001f);
            Assert.AreEqual(tr.x, max.x, 0.001f);
            Assert.AreEqual(tr.y, max.y, 0.001f);
        }

        [Test]
        public void IsInsideBounds_ShouldReturnTrue_WhenInside()
        {
            var boundsService = new CameraBoundsService(testCamera);

            Vector2 center = Vector2.zero;
            Assert.IsTrue(boundsService.IsInsideBounds(center));
        }
    }
}