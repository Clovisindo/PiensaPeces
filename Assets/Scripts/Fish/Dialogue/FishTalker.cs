using Assets.Scripts.Components;
using Assets.Scripts.Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Fish.Dialogue
{
    public class FishTalker : MonoBehaviour
    {
        [SerializeField] private string dialogueCsvPath;
        [SerializeField] private float minSpeakInterval = 5f;

        [SerializeField] private GameObject speechBubblePrefab;
        [SerializeField] private Transform bubbleAnchor;

        private float lastSpeakTime;
        private List<FishDialogueLine> dialogueLines;
        private FishDialogueEvaluator evaluator;

        public void Init(HungerComponent hunger)
        {
            dialogueLines = DialogueLoaderCsv.Load(Application.dataPath + "/Scripts/Data/" + dialogueCsvPath);
            evaluator = new FishDialogueEvaluator(hunger);
            lastSpeakTime = -minSpeakInterval;
        }

        private void Update()
        {
            if (Time.time - lastSpeakTime < minSpeakInterval) return;

            foreach (var line in dialogueLines)
            {
                if (evaluator.Evaluate(line.Condition))
                {
                    Speak(line.Text);
                    lastSpeakTime = Time.time;
                    break;
                }
            }
        }

        private void Speak(string text)
        {
            if (speechBubblePrefab != null && bubbleAnchor != null)
            {
                var bubble = Instantiate(speechBubblePrefab, bubbleAnchor.position, Quaternion.identity, null);
                bubble.transform.SetParent(bubbleAnchor.transform);
                bubble.GetComponent<SpeechBubbleUI>().Show(text);
            }

            Debug.Log($"[Fish] {text}");
        }
    }

}
