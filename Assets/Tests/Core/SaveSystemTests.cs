using NUnit.Framework;
using System;
using System.IO;

namespace Game.Core.Tests
{
    public class SaveSystemTests
    {
        private MockFileStorage storage;
        private SaveSystem saveSystem;

        [SetUp]
        public void Setup()
        {
            storage = new MockFileStorage();
            saveSystem = new SaveSystem(storage);
        }

        [Test]
        public void FirstLaunch_ShouldWriteDate_AndReturnTrue()
        {
            var testPath = Path.Combine(Path.GetTempPath(), "ResetLaunchTest.marker");//usamos GetTempPath para tener rutas distintas en cada test
            saveSystem = new SaveSystem(storage, testPath);
            
            var isFirst = saveSystem.IsFirstLaunch();
            Assert.IsTrue(isFirst);
            Assert.IsTrue(storage.Exists(testPath));

            var storedDate = DateTime.FromBinary(Convert.ToInt64(storage.GetContent(testPath)));
            Assert.That((DateTime.Now - storedDate).TotalSeconds, Is.LessThan(2));
        }

        [Test]
        public void ResetFirstLaunch_ShouldDeleteTheMarker()
        {
            // Arrange
            var testPath = Path.Combine(Path.GetTempPath(), "ResetLaunchTest.marker");
            if (File.Exists(testPath))
                File.Delete(testPath);
            var saveSystem = new SaveSystem(storage, testPath);
            // preparamos y comprobamos el fichero
            saveSystem.GetFirstLaunchDate();
            Assert.IsTrue(storage.Exists(testPath));

            // Act
            saveSystem.ResetFirstLaunch();

            // Assert
            Assert.IsFalse(storage.Exists(testPath), "Expected marker file to be deleted");
        }

        [Test]
        public void SecondLaunch_ShouldReturnFalse()
        {
            saveSystem.IsFirstLaunch();

            var second = saveSystem.IsFirstLaunch();
            Assert.IsFalse(second);
        }

        [Test]
        public void CanSetCustomLaunchDate_AndRetrieveIt()
        {
            var customDate = new DateTime(2020, 1, 1);
            saveSystem.SetFirstLaunchDate(customDate);

            var result = saveSystem.GetFirstLaunchDate();
            Assert.AreEqual(customDate, result);
        }

        [Test]
        public void GetDaysSinceFirstLaunch_ShouldBeCorrect()
        {
            var now = DateTime.Now;
            var daysAgo = now.AddDays(-3);
            saveSystem.SetFirstLaunchDate(daysAgo);

            var result = saveSystem.GetDaysSinceFirstLaunch();
            Assert.AreEqual(4, result); // +1 porque se cuenta desde el día 0 como 1
        }

        
    }
}