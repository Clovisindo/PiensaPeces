using Assets.Scripts.Fish.Dialogue;
using Game.Data;
using Game.Events;
using Game.Services;
using Game.Utilities;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Game.Fishes.Tests
{

    public class FishTalkerTests 
    {
        private GameObject go;
        private FishTalker fishTalker;
        private IDialogueEvaluator dialogueEvaluator;
        private IResourceLoader resourceLoader;
        private IGameObjectFactory gameObjectFactory;
        private IRandomService randomService;
        private IDialoguePathResolver dialoguePathResolver;
        private ITimeService timeService;
        private IGlobal globalSpeed;
        private FishConfig config;
        private IEventBus<SFXEvent> eventBus;
        private readonly int passedDays = 1;
        private const string rootPath = "Dialogues/";
        private const string baseNameFile = "fakePath";
        string nameFileWithDay;


        [SetUp]
        public void Setup()
        {
            go = new GameObject();
            fishTalker = go.AddComponent<FishTalker>();

            dialogueEvaluator = Substitute.For<IDialogueEvaluator>();
            resourceLoader = Substitute.For<IResourceLoader>();
            gameObjectFactory = Substitute.For<IGameObjectFactory>();
            randomService = Substitute.For<IRandomService>();
            eventBus = Substitute.For<IEventBus<SFXEvent>>();
            timeService = Substitute.For<ITimeService>();

            globalSpeed = Substitute.For<IGlobal>();
            globalSpeed.GameSpeed.Returns(1f);

            dialoguePathResolver = Substitute.For<IDialoguePathResolver>();
            dialoguePathResolver.Resolver(dialogueEvaluator, Arg.Any<string>(), Arg.Any<string>()).Returns(baseNameFile);
            nameFileWithDay = $"{baseNameFile}_day{passedDays}";

            config = ScriptableObject.CreateInstance<FishConfig>();
            config.intervalTalking = 2f;
            config.sftTalk = ScriptableObject.CreateInstance<AudioEmitterData>();
            
            randomService.Range(Arg.Any<float>(), Arg.Any<float>()).Returns(1f);
            dialogueEvaluator.Evaluate(Arg.Any<string>()).Returns(true);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(go);
        }

        [Test]
        public void Init_ShouldLoadDialoguesCurrentDay_AndResetTimer()
        {
            var fakeCsv = new TextAsset("id,text,condicion\n1,Hello from fake day1,true");
            resourceLoader.LoadText(Arg.Any<string>()).Returns(fakeCsv);
            InitFishTalkerAndDependencies();

            resourceLoader.Received(1).LoadText(rootPath + nameFileWithDay);
            dialoguePathResolver.Received(1).Resolver(dialogueEvaluator, Arg.Any<string>(), Arg.Any<string>());
            randomService.Received(2).Range(Arg.Any<float>(), Arg.Any<float>());// en init() y en resetTalker()
        }

        [Test]
        public void LoadDialogues_WhenFileDontExist_ThenLoadDefault()
        {
            var baseCsv = new TextAsset("id,text,condicion\n1,Hello from base,true");
            resourceLoader.LoadText(rootPath + nameFileWithDay).Returns((TextAsset)null);
            resourceLoader.LoadText(rootPath + baseNameFile).Returns(baseCsv);

            InitFishTalkerAndDependencies();

            Received.InOrder(() =>
            {
                resourceLoader.Received(1).LoadText(rootPath + nameFileWithDay);
                resourceLoader.Received(1).LoadText(rootPath + baseNameFile);
            });
            dialoguePathResolver.Received(1).Resolver(dialogueEvaluator, Arg.Any<string>(), Arg.Any<string>());
            randomService.Received(2).Range(Arg.Any<float>(), Arg.Any<float>());// en init() y en resetTalker()
        }

        [Test]
        public void LoadDialogues_WhenDefaultDontExist_ThenDialogueLinesEmpty()
        {
            resourceLoader.LoadText(rootPath + nameFileWithDay).Returns((TextAsset)null);
            resourceLoader.LoadText(rootPath + baseNameFile).Returns((TextAsset)null);

            InitFishTalkerAndDependencies();

            Received.InOrder(() =>
            {
                resourceLoader.Received(1).LoadText(rootPath + nameFileWithDay);
                resourceLoader.Received(1).LoadText(rootPath + baseNameFile);
            });
            dialoguePathResolver.Received(1).Resolver(dialogueEvaluator, Arg.Any<string>(), Arg.Any<string>());
            randomService.Received(2).Range(Arg.Any<float>(), Arg.Any<float>());// en init() y en resetTalker()
        }

        [Test]
        public void PathResolver_LoadCorrectPath()
        {
            var playerEvaluator = new PlayerFishDialogueEvaluator(new HungerComponent());
            var pathResolver = new DialoguePathResolver();
            var result = pathResolver.Resolver(playerEvaluator, "playerPath", "npcPath");

            Assert.AreEqual("playerPath",result);
            var npcEvaluator = new NPCFishDialogueEvaluator();
            result = pathResolver.Resolver(npcEvaluator, "playerPath", "npcPath");
            Assert.AreEqual("npcPath", result);
        }

        [Test]
        public void ResetTalker_WhenHaveActiveBubble_ThenDestroyAndRandomCalled()
        {
            InitFishTalkerAndDependencies();
            var child = new GameObject("bubble");
            child.AddComponent<SpeechBubbleUI>();
            child.transform.parent = fishTalker.transform;

            fishTalker.ResetTalker();

            gameObjectFactory.Received(1).Destroy(Arg.Is<GameObject>( go => go == child));
            randomService.Received(1);
        }

        [Test]//para playmode
        public void Update_WhenNoSpeakDelayPassed_ThenNoSpeak()
        {
            InitFishTalkerAndDependencies();
            fishTalker.Tick(timeService.DeltaTime);

            gameObjectFactory.DidNotReceive().Instantiate(Arg.Any<GameObject>(),Arg.Any<Vector3>(),Arg.Any<Quaternion>());
            eventBus.DidNotReceive().Raise(Arg.Any<SFXEvent>());
            dialogueEvaluator.DidNotReceive().Evaluate(Arg.Any<string>());
        }


        private void InitFishTalkerAndDependencies()
        {
            var dependenciesFishTalker = new FishTalkerDependencies(
                            dialogueEvaluator,
                            dialoguePathResolver,
                            resourceLoader,
                            gameObjectFactory,
                            timeService,
                            globalSpeed,
                            eventBus,
                            config,
                            passedDays,
                            randomService);

            fishTalker.Init(dependenciesFishTalker);
        }
    }
}