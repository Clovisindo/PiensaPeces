using Game.Data;
using Game.Utilities;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Services.Tests
{

    public class EnviromentSystemTests
    {
        private TestLogger logger;
        private EnviromentLoader loaderService;

        [SetUp]
        public void SetUp()
        {
            logger = new TestLogger();
            loaderService = new EnviromentLoader(logger);
        }

        [Test]
        public void Load_ReturnsSameConfigs_WhenDayExists()
        {
            var groundDay = ScriptableObject.CreateInstance<GroundEnvironmentDayConfig>();
            groundDay.dayNumber = 1;
            groundDay.groundEnvConfigs = new List<GroundEnvConfig>
            {
                new GroundEnvConfig { prefab = new GameObject("p1"), sprite = null }
            };

            var fishDay = ScriptableObject.CreateInstance<FishEnvDayConfig>();
            fishDay.dayNumber = 1;
            fishDay.fishEnvDayConfigs = new List<FishConfig> { ScriptableObject.CreateInstance<FishConfig>() };

            var foodDay = ScriptableObject.CreateInstance<FoodEnvDayConfig>();
            foodDay.dayNumber = 1;
            foodDay.foodEnvConfigs = new List<FoodEnvConfig> { ScriptableObject.CreateInstance<FoodEnvConfig>() };

            var audioDay = ScriptableObject.CreateInstance<AudioEnvDayConfig>();
            audioDay.dayNumber = 1;
            audioDay.audioConfigs = new List<AudioEnvConfig> { ScriptableObject.CreateInstance<AudioEnvConfig>() };

            var result = loaderService.Load(
                new List<GroundEnvironmentDayConfig> { groundDay },
                new List<FishEnvDayConfig> { fishDay },
                new List<FoodEnvDayConfig> { foodDay },
                new List<AudioEnvDayConfig> { audioDay },
                1);

            Assert.IsNotNull(result);
            Assert.AreSame(groundDay, result.SelectedGroundConfig);
            Assert.AreSame(fishDay.fishEnvDayConfigs.First(), result.Context.FishConfigsCurrentDay.First());
            Assert.AreSame(foodDay.foodEnvConfigs.First(), result.Context.FoodConfigsCurrentDay.First());
            Assert.AreSame(audioDay.audioConfigs.First(), result.Context.AudioConfigsCurrentDay.First());
            Assert.AreEqual(1, result.Context.FishConfigsCurrentDay.Length);
            Assert.AreEqual(1, result.Context.FoodConfigsCurrentDay.Length);
            Assert.AreEqual(1, result.Context.AudioConfigsCurrentDay.Length);
        }

        [Test]
        public void Load_UsesLatestDay_WhenDayNotFound()
        {
            var groundDay1 = ScriptableObject.CreateInstance<GroundEnvironmentDayConfig>();
            groundDay1.dayNumber = 1;

            var groundDay3 = ScriptableObject.CreateInstance<GroundEnvironmentDayConfig>();
            groundDay3.dayNumber = 3;

            var result = loaderService.Load(
                new List<GroundEnvironmentDayConfig> { groundDay1, groundDay3 },
                new List<FishEnvDayConfig>(),
                new List<FoodEnvDayConfig>(),
                new List<AudioEnvDayConfig>(),
                2); // pide día 2, no existe

            Assert.IsNotNull(result.SelectedGroundConfig);
            Assert.AreEqual(3, result.SelectedGroundConfig.dayNumber);
        }

        [Test]
        public void Load_ReturnsEmptyArrays_WhenListsEmpty()
        {
            var result = loaderService.Load(
                new List<GroundEnvironmentDayConfig>(),
                new List<FishEnvDayConfig>(),
                new List<FoodEnvDayConfig>(),
                new List<AudioEnvDayConfig>(),
                1);

            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Context.FishConfigsCurrentDay);
            Assert.IsEmpty(result.Context.FoodConfigsCurrentDay);
            Assert.IsEmpty(result.Context.AudioConfigsCurrentDay);
            Assert.IsNull(result.SelectedGroundConfig);
        }

        [Test]
        public void LoadEnvironment_ShouldLogWarning_WhenNoGroundConfigs()
        {
            loaderService.Load(
                new List<GroundEnvironmentDayConfig>(),
                new List<FishEnvDayConfig>(),
                new List<FoodEnvDayConfig>(),
                new List<AudioEnvDayConfig>(),
                1
            );

            Assert.IsTrue(logger.Warnings.Any());//ToDo: para que esto este bien , deberian ser categorias no mensajes concretos
        }
    }
}
