using Game.Data;
using Game.Utilities;
using System.Collections;
using UnityEngine;

namespace Game.Fishes
{
    public class SpeechBubbleLogic
    {
        private readonly SpeechBubbleConfig _config;
        private readonly ITextMeshProWrapper _textWrapper;
        private readonly ICanvasGroupWrapper _canvasWrapper;
        private readonly IRectTransformWrapper _rectWrapper;
        private readonly ITimeService _timeService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IGameObjectFactory _gameObjectFactory;

        public SpeechBubbleLogic(SpeechBubbleConfig config,
                               ITextMeshProWrapper textWrapper,
                               ICanvasGroupWrapper canvasWrapper,
                               IRectTransformWrapper rectWrapper,
                               ITimeService timeService,
                               ICoroutineRunner coroutineRunner,
                               IGameObjectFactory gameObjectFactory)
        {
            _config = config;
            _textWrapper = textWrapper;
            _canvasWrapper = canvasWrapper;
            _rectWrapper = rectWrapper;
            _timeService = timeService;
            _coroutineRunner = coroutineRunner;
            _gameObjectFactory = gameObjectFactory;
        }

        public void Show(string text, GameObject gameObject)
        {
            SetupText(text);
            AdjustSize();
            _coroutineRunner.StartDisplayCoroutine(ShowRoutine(gameObject));
        }

        private void SetupText(string text)
        {
            _textWrapper.FontSize = _config.FontSize;
            _textWrapper.SetText(text);
            _textWrapper.ForceMeshUpdate();
        }

        private void AdjustSize()
        {
            Vector2 textSize = _textWrapper.GetRenderedValues(false);
            _rectWrapper.SizeDelta = textSize + _config.Padding;
        }

        private IEnumerator ShowRoutine(GameObject gameObject)
        {
            yield return Fade(0, 1); // fade in
            yield return WaitForSeconds(_config.DisplayDuration);
            yield return Fade(1, 0); // fade out
            _gameObjectFactory.Destroy(gameObject);
        }

        private IEnumerator Fade(float from, float to)
        {
            float elapsed = 0f;
            while (elapsed < _config.FadeDuration)
            {
                elapsed += _timeService.DeltaTime;
                _canvasWrapper.Alpha = Mathf.Lerp(from, to, elapsed / _config.FadeDuration);
                yield return null;
            }
            _canvasWrapper.Alpha = to;
        }

        private IEnumerator WaitForSeconds(float seconds)
        {
            float elapsed = 0f;
            while (elapsed < seconds)
            {
                elapsed += _timeService.DeltaTime;
                yield return null;
            }
        }
    }
}
