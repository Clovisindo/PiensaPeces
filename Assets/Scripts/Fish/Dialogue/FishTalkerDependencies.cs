using Game.Data;
using Game.Events;
using Game.Fishes;
using Game.Services;
using Game.Utilities;

namespace Assets.Scripts.Fish.Dialogue
{
    public class FishTalkerDependencies
    {
        public IDialogueEvaluator Evaluator { get; }
        public IDialoguePathResolver PathResolver { get; }
        public IResourceLoader ResourceLoader { get; }
        public IGameObjectFactory GameObjectFactory { get; }
        public IRandomService RandomService { get; }
        public ITimeService TimeService { get; }
        public IEventBus<SFXEvent> EventBus { get; }
        public IGlobal Global { get; }

        public FishConfig Config { get; }
        public int PassedDays { get; }

        public FishTalkerDependencies(IDialogueEvaluator evaluator,
            IDialoguePathResolver pathResolver,
            IResourceLoader resourceLoader,
            IGameObjectFactory gameObjectFactory,
            ITimeService timeService,
            IGlobal global,
            IEventBus<SFXEvent> eventBus,
            FishConfig config,
            int passedDays,
            IRandomService randomService = null
            )
        {
            Evaluator = evaluator;
            PathResolver = pathResolver;
            ResourceLoader = resourceLoader;
            GameObjectFactory = gameObjectFactory;
            RandomService = randomService ?? new UnityRandomService();
            TimeService = timeService;
            EventBus = eventBus;
            Global = global;
            Config = config;
            PassedDays = passedDays;
        }
    }
}
