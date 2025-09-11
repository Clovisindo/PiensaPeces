using Game.Data;
using Game.Utilities;
using TMPro;
using UnityEngine;

namespace Game.Fishes
{
    public class SpeechBubbleUI : MonoBehaviour,ISpeechBubbleUI
    {
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float displayDuration = 2f;
        [SerializeField] private float fadeDuration = 0.5f;

        private SpeechBubbleLogic speechBubbleLogic;

        private ITimeService _timerService;
        private ICoroutineRunner _coroutineRunner;
        private IGameObjectFactory _factory;

        public void Initialize(ITimeService timeProvider = null,
                     ICoroutineRunner coroutineRunner = null,
                     IGameObjectFactory destroyer = null)
        {
            _timerService = timeProvider ?? new UnityTimeService();
            _coroutineRunner = coroutineRunner ?? new UnityCoroutineRunner(this);
            _factory = destroyer ?? new UnityGameObjectFactory();

            var config = new SpeechBubbleConfig
            {
                FontSize = 36f * 5,
                DisplayDuration = displayDuration,
                FadeDuration = fadeDuration,
                Padding = new Vector2(20f, 20f)
            };

            var textWrapper = new TextMeshProWrapper(dialogueText);
            var canvasWrapper = new CanvasGroupWrapper(canvasGroup);
            var rectWrapper = new RectTransformWrapper(canvasGroup.GetComponent<RectTransform>());

            speechBubbleLogic = new SpeechBubbleLogic(config, textWrapper, canvasWrapper, rectWrapper,
                                       _timerService, _coroutineRunner, _factory);
        }

        public void InitForTests()
        {
            dialogueText = gameObject.AddComponent<TextMeshProUGUI>();
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            Initialize();
        }
        //private void Awake()
        //{
        //    if (speechBubbleLogic == null)
        //        Initialize();
        //}

        public void Show(string text)
        {
            speechBubbleLogic.Show(text, gameObject);
        }
    }

}
