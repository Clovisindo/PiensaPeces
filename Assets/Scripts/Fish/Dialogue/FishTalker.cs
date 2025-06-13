using Assets.Scripts.Components;
using Assets.Scripts.Core;
using Assets.Scripts.Events.EventBus;
using Assets.Scripts.Events.Events;
using Assets.Scripts.Fish.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Assets.Scripts.Fish.Dialogue
{
    public class FishTalker : MonoBehaviour
    {
        [SerializeField] private string playerDialogueCsvPath;
        [SerializeField] private string npcDialogueCsvPath;
        [SerializeField] private float minSpeakInterval;
        private float nextSpeakTime = 0f;
        private float intervalEvaluateIntent;

        [SerializeField] private GameObject speechBubblePrefab;
        [SerializeField] private Transform bubbleAnchor;

        private EventBus<SFXEvent> eventBus;
        AudioEmitterData talkingSFX;
        private float lastSpeakTime;
        private List<FishDialogueLine> dialogueLines;
        private IDialogueEvaluator evaluator;
        private SpeechBubbleUI activeBubble;

        private const string pathData = "Dialogues/";

        public void Init(IDialogueEvaluator evaluator, FishConfig config, EventBus<SFXEvent> sfxEventBus)
        {
            this.eventBus = sfxEventBus;
            this.talkingSFX = config.sftTalk;
            this.minSpeakInterval = config.intervalTalking;
            string pathDialoge = evaluator switch
            {
                PlayerFishDialogueEvaluator => playerDialogueCsvPath,
                NPCFishDialogueEvaluator => npcDialogueCsvPath,
                _ => throw new ArgumentException("Unknown evaluator type")
            };
            TextAsset csvAsset = Resources.Load<TextAsset>(pathData + pathDialoge);
            if (csvAsset == null)
            {
                Debug.LogError($"Dialogue CSV not found at Resources/{pathData + pathDialoge}");
                dialogueLines = new List<FishDialogueLine>();
            }
            else
            {
                dialogueLines = DialogueLoaderCsv.Load(csvAsset.text);
            }
            this.evaluator = evaluator;
            this.intervalEvaluateIntent = config.intervalEvaluateIntent;
            nextSpeakTime = intervalEvaluateIntent; 
            ResetTalker();
        }

        public void ResetTalker()
        {
            nextSpeakTime = intervalEvaluateIntent;
            activeBubble = gameObject.GetComponentInChildren<SpeechBubbleUI>();
            if (activeBubble != null)
            {
                Destroy(activeBubble.gameObject);
                activeBubble = null;
            }
        }

        private void Update()
        {
            if (Time.time < nextSpeakTime) return;

            var validLines = dialogueLines.Where(line => evaluator.Evaluate(line.Condition)).ToList();
            if (validLines.Count == 0) return;

            activeBubble = gameObject.GetComponentInChildren<SpeechBubbleUI>();
            if (activeBubble != null)
            {
                Debug.LogWarning("Ya hay una burbuja activa y se iba a hablar.");
                return;
            }

            var selectedLine = validLines[UnityEngine.Random.Range(0, validLines.Count)];
            Speak(selectedLine.Text, talkingSFX);
            nextSpeakTime = Time.time + minSpeakInterval;
        }

        private void Speak(string text, AudioEmitterData audioData)
        {
            if (speechBubblePrefab != null && bubbleAnchor != null)
            {
                var bubble = Instantiate(speechBubblePrefab, bubbleAnchor.position, Quaternion.identity, null);
                bubble.transform.SetParent(bubbleAnchor.transform);
                bubble.GetComponent<SpeechBubbleUI>().Show(text);
                eventBus.Raise(new SFXEvent { sfxData = audioData});
            }
        }
    }

}
