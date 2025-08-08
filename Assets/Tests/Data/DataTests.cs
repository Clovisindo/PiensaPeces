using Assets.Tests.Data.Mocks;
using Game.Context;
using Game.Services;
using Game.Utilities;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data.Tests
{
    public class DataTests 
    {
        private const int AUDIO_DAYS = 1;
        private const int FISHS_DAYS = 1;
        private const int FOOD_DAYS = 1;
        private const int GROUND_DAYS = 1;
        private const string AUDIO_NAME = "Audio test";
        private const float AUDIO_TRIGGER_VALUE = 5.5f;
        private const float FISH_SPEED = 1f;
        private const float FISH_MAX_LIFETIME = 10f;
        private const float FISH_SWIM_PROB = 0.5f;
        private const float FISH_INTERVAL_TALK = 5f;
        private const float FISH_INTERVAL_EVALUATE_INTENT = 5f;


        [Test]
        public void AudioEmitterData_Should_Have_Correct_Values()
        {
            var config = ScriptableObject.CreateInstance<AudioEmitterData>();

            config.AudioName = AUDIO_NAME;
            var gameObject = new GameObject("TestAudioSource");
            var audioSource = gameObject.AddComponent<AudioSource>();
            config.AudioSource = audioSource;
            config.InstancePrefab = audioSource;

            Assert.AreEqual(AUDIO_NAME, config.AudioName);
            Assert.IsNotNull(config.AudioSource);
            Assert.IsNotNull(config.InstancePrefab);
        }

        [Test]
        public void AudioEnvConfig_Should_Have_Correct_Values()
        {
            var config = ScriptableObject.CreateInstance<AudioEnvConfig>();

            config.TriggerCondition = AudioTriggerCondition.TimePassedMinutes;
            config.TriggerValue = AUDIO_TRIGGER_VALUE;
            config.AudioEmitterData = ScriptableObject.CreateInstance<AudioEmitterData>();

            Assert.AreEqual(AudioTriggerCondition.TimePassedMinutes, config.TriggerCondition);
            Assert.AreEqual(AUDIO_TRIGGER_VALUE, config.TriggerValue);
            Assert.IsNotNull(config.AudioEmitterData);
        }

        [Test]
        public void FishConfig_Should_Have_Correct_Values()
        {
            var config = ScriptableObject.CreateInstance<FishConfig>();
            var texture = new Texture2D(32, 32);
            var sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), Vector2.zero);
            config.fishSprite = sprite;
            config.speed = FISH_SPEED;
            config.maxLifetime = FISH_MAX_LIFETIME;
            config.swimProbability = FISH_SWIM_PROB;
            config.intervalTalking = FISH_INTERVAL_TALK;
            config.intervalEvaluateIntent = FISH_INTERVAL_EVALUATE_INTENT;
            config.sftTalk = ScriptableObject.CreateInstance<AudioEmitterData>();


            Assert.IsNotNull(config.fishSprite);
            Assert.AreEqual(FISH_SPEED, config.speed);
            Assert.AreEqual(FISH_MAX_LIFETIME, config.maxLifetime);
            Assert.AreEqual(FISH_SWIM_PROB, config.swimProbability);
            Assert.AreEqual(FISH_INTERVAL_TALK, config.intervalTalking);
            Assert.AreEqual(FISH_INTERVAL_EVALUATE_INTENT, config.intervalEvaluateIntent);
            Assert.IsNotNull(config.sftTalk);
        }

        [Test]
        public void FoodEnvConfig_Should_Have_Correct_Values()
        {
            var config = ScriptableObject.CreateInstance<FoodEnvConfig>();
            var texture = new Texture2D(32, 32);
            var sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), Vector2.zero);
            config.prefab = new GameObject();
            config.sprite = sprite;

            Assert.IsNotNull(config.prefab);
            Assert.IsNotNull(config.sprite);
        }
        
        [Test]
        public void GroundEnvConfig_Should_Have_Correct_Values()
        {
            var config = ScriptableObject.CreateInstance<GroundEnvConfig>();
            var texture = new Texture2D(32, 32);
            var sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), Vector2.zero);
            config.prefab = new GameObject();
            config.sprite = sprite;

            Assert.IsNotNull(config.prefab);
            Assert.IsNotNull(config.sprite);
        }

        [Test]
        public void AudioEnvDayConfig_Should_Have_Correct_Values()
        {
            var config = ScriptableObject.CreateInstance<AudioEnvDayConfig>();

            config.dayNumber = AUDIO_DAYS;
            config.audioConfigs = new List<AudioEnvConfig>();

            Assert.AreEqual(AUDIO_DAYS, config.dayNumber);
            Assert.IsNotNull(config.audioConfigs);
        }

        [Test]
        public void FishEnvDayConfig_Should_Have_Correct_Values()
        {
            var config = ScriptableObject.CreateInstance<FishEnvDayConfig>();

            config.dayNumber = FISHS_DAYS;
            config.fishEnvDayConfigs = new List<FishConfig>();

            Assert.AreEqual(FISHS_DAYS, config.dayNumber);
            Assert.IsNotNull(config.fishEnvDayConfigs);
        }

        [Test]
        public void FoodEnvDayConfig_Should_Have_Correct_Values()
        {
            var config = ScriptableObject.CreateInstance<FoodEnvDayConfig>();
            config.dayNumber = FOOD_DAYS;
            config.foodEnvConfigs = new List<FoodEnvConfig>();

            Assert.AreEqual(FOOD_DAYS, config.dayNumber);
            Assert.IsNotNull(config.foodEnvConfigs);
        }

        [Test]
        public void GroundEnvironmentDayConfig_Should_Have_Correct_Values()
        {
            var config = ScriptableObject.CreateInstance<GroundEnvironmentDayConfig>();
            config.dayNumber = GROUND_DAYS;
            config.groundEnvConfigs = new List<GroundEnvConfig>();

            Assert.AreEqual(GROUND_DAYS, config.dayNumber);
            Assert.IsNotNull(config.groundEnvConfigs);
        }

        [Test]
        public void ExitScreenContext_Should_Have_Correct_Values()
        {
            var gameObject = new GameObject();
            var mockBoundService = new MockBoundService();
            var mockExitableFIsh = new MockExitable();

            var fishExitScreenContext = new ExitScreenContext(gameObject.transform,mockBoundService, mockExitableFIsh, FISH_SPEED);

            Assert.AreEqual(FISH_SPEED, fishExitScreenContext.Speed);
            Assert.IsNotNull(fishExitScreenContext.Transform);
            Assert.IsNotNull(fishExitScreenContext.BoundsService);
            Assert.IsNotNull(fishExitScreenContext.ExitBehavior);
        }

        [Test]
        public void Build_ShouldAssignFishConfigsCorrectly()
        {
            var fish1 = ScriptableObject.CreateInstance<FishConfig>();
            var fish2 = ScriptableObject.CreateInstance<FishConfig>();
            var fishArray = new[] { fish1, fish2 };

            var context = new LoadDataContext.Builder()
                .WithFishConfigs(fishArray)
                .Build();

            Assert.AreEqual(2, context.FishConfigsCurrentDay.Length);
            Assert.AreSame(fish1, context.FishConfigsCurrentDay[0]);
            Assert.AreSame(fish2, context.FishConfigsCurrentDay[1]);
        }

        [Test]
        public void Build_ShouldAssignFoodConfigsCorrectly()
        {
            var food = ScriptableObject.CreateInstance<FoodEnvConfig>();
            var context = new LoadDataContext.Builder()
                .WithFoodConfigs(new[] { food })
                .Build();

            Assert.AreEqual(1, context.FoodConfigsCurrentDay.Length);
            Assert.AreSame(food, context.FoodConfigsCurrentDay[0]);
        }

        [Test]
        public void Build_ShouldAssignAudioConfigsCorrectly()
        {
            var audio = ScriptableObject.CreateInstance<AudioEnvConfig>();
            var context = new LoadDataContext.Builder()
                .WithAudioConfigs(new[] { audio })
                .Build();

            Assert.AreEqual(1, context.AudioConfigsCurrentDay.Length);
            Assert.AreSame(audio, context.AudioConfigsCurrentDay[0]);
        }

        [Test]
        public void Build_ShouldUseEmptyArrays_WhenNoValuesProvided()
        {
            var context = new LoadDataContext.Builder().Build();

            Assert.IsNotNull(context.FishConfigsCurrentDay);
            Assert.IsEmpty(context.FishConfigsCurrentDay);
            Assert.IsNotNull(context.FoodConfigsCurrentDay);
            Assert.IsEmpty(context.FoodConfigsCurrentDay);
            Assert.IsNotNull(context.AudioConfigsCurrentDay);
            Assert.IsEmpty(context.AudioConfigsCurrentDay);
        }

        [Test]
        public void Build_ShouldUseEmptyArrays_WhenPassingNulls()
        {
            var context = new LoadDataContext.Builder()
                .WithFishConfigs(null)
                .WithFoodConfigs(null)
                .WithAudioConfigs(null)
                .Build();

            Assert.IsNotNull(context.FishConfigsCurrentDay);
            Assert.IsEmpty(context.FishConfigsCurrentDay);
            Assert.IsNotNull(context.FoodConfigsCurrentDay);
            Assert.IsEmpty(context.FoodConfigsCurrentDay);
            Assert.IsNotNull(context.AudioConfigsCurrentDay);
            Assert.IsEmpty(context.AudioConfigsCurrentDay);
        }
    }
}
