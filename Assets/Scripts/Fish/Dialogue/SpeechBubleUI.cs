using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Fishes
{
    public class SpeechBubbleUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float displayDuration = 2f;
        [SerializeField] private float fadeDuration = 0.5f;

        private Coroutine displayRoutine;

        public void Show(string text)
        {
            dialogueText.fontSize = 36f * 5; 
            dialogueText.SetText(text);
            dialogueText.ForceMeshUpdate();
            Vector2 textSize = dialogueText.GetRenderedValues(false);
            // Obtener el RectTransform del objeto que contiene el texto
            RectTransform rectTransform = canvasGroup.GetComponent<RectTransform>();
            // Ajustar el tamaño del rectángulo para que sea un poco más grande que el texto
            rectTransform.sizeDelta = textSize + new Vector2(20f, 20f);

            if (displayRoutine != null)
                StopCoroutine(displayRoutine);

            displayRoutine = StartCoroutine(ShowRoutine());
        }

        private IEnumerator ShowRoutine()
        {
            yield return Fade(0, 1); // fade in
            yield return new WaitForSeconds(displayDuration);
            yield return Fade(1, 0); // fade out
            Destroy(gameObject);
        }

        private IEnumerator Fade(float from, float to)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = to;
        }
    }

}
