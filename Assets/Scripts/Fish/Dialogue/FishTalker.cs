using Assets.Scripts.Fish.Dialogue;
using Game.Core;
using Game.Data;
using Game.Events;
using Game.Services;
using Game.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Fishes
{
    public class FishTalker : MonoBehaviour
    {
        [Header("Editor Configuration")]
        [SerializeField] private string playerDialogueCsvPath;
        [SerializeField] private string npcDialogueCsvPath;
        [SerializeField] private float minSpeakInterval;
        [SerializeField] private float maxSpeakInterval;
        [SerializeField] private GameObject speechBubblePrefab;
        [SerializeField] private Transform bubbleAnchor;

        private FishTalkerDependencies Dependencies;
        AudioEmitterData talkingSFX;
        private List<FishDialogueLine> dialogueLines;
        private SpeechBubbleUI activeBubble;

        private float elapsedTime = 0f;
        private float nextSpeakDelay = 0f;
        private const string rootPathData = "Dialogues/";
        private bool isInitialized = false;

        public void Init(FishTalkerDependencies dependencies)
        {
            InitInternal(dependencies, speechBubblePrefab, bubbleAnchor, minSpeakInterval, minSpeakInterval * 1.5f);
        }

        public void InitForTesting(FishTalkerDependencies dependencies,
            GameObject bublePrefab,
            Transform anchor,
            string playerPath = "player_dialogue",
            string npcPatch = "npc_dialogue",
            float minInterval = 1.0f,
            float maxInterval = 2.0f)
        {
            playerDialogueCsvPath = playerPath;
            npcDialogueCsvPath = npcPatch;
            InitInternal(dependencies, bublePrefab, anchor, minInterval, maxInterval);
        }

        private void InitInternal(FishTalkerDependencies dependencies,
            GameObject bublePrefab,
            Transform anchor,
            float minInterval,
            float maxInterval)
        {
            Dependencies = dependencies;
            speechBubblePrefab = bublePrefab;
            bubbleAnchor = anchor;
            talkingSFX = Dependencies.Config.sftTalk;
            minSpeakInterval = minInterval;
            maxSpeakInterval = maxInterval;
            nextSpeakDelay = Dependencies.RandomService.Range(minSpeakInterval, maxSpeakInterval) * Dependencies.Global.GameSpeed;
            
            string pathDialoge = Dependencies.PathResolver.Resolver(Dependencies.Evaluator, playerDialogueCsvPath, npcDialogueCsvPath);
            LoadDialogueAssets(pathDialoge, Dependencies.PassedDays);
            
            ResetTalker();
            isInitialized = true;
        }

        private void LoadDialogueAssets(string baseNameFile, int passedDays)
        {
            // Agregar sufijo de día al nombre del archivo
            string pathDialogeWithDay = $"{baseNameFile}_day{passedDays}";

            // Cargar CSV
            TextAsset csvAsset = Dependencies.ResourceLoader.LoadText(rootPathData + pathDialogeWithDay);


            if (csvAsset == null)
            {
                Debug.LogWarning($"Dialogue CSV not found at Resources/{rootPathData + pathDialogeWithDay}, attempting fallback to default.");

                // Fallback a base CSV si no existe el específico
                csvAsset = Dependencies.ResourceLoader.LoadText(rootPathData + baseNameFile);
            }

            if (csvAsset == null)
            {
                Debug.LogWarning($"Default dialogue CSV also not found at Resources/{rootPathData + baseNameFile}");
                dialogueLines = new List<FishDialogueLine>();
            }
            else
            {
                dialogueLines = DialogueLoaderCsv.Load(csvAsset.text);
            }
        }

        private void Update()
        {
            // GUARD CLAUSE: No ejecutar si no está inicializado
            if (!isInitialized || Dependencies == null)
            {
                return;
            }
            bool flowControl = Tick(Dependencies.TimeService.DeltaTime);
            if (!flowControl)
            {
                return;
            }
        }

        public bool Tick( float deltaTime)
        {
            elapsedTime += deltaTime;
            if (elapsedTime < nextSpeakDelay) return false;
            var validLines = dialogueLines.Where(line => Dependencies.Evaluator.Evaluate(line.Condition)).ToList();
            if (validLines.Count == 0) return false;

            var selectedLine = validLines[Dependencies.RandomService.Range(0, validLines.Count)];
            Speak(selectedLine.Text, talkingSFX);
            ResetTimer();
            return true;
        }

        private bool CheckActiveBuble()
        {
            activeBubble = gameObject.GetComponentInChildren<SpeechBubbleUI>();
            if (activeBubble != null)
            {
                ResetTimer();
                Debug.LogWarning("Ya hay una burbuja activa y se iba a hablar.");
                return true;
            }
            else return false;
        }

        private void Speak(string text, AudioEmitterData audioData)
        {
            if(CheckActiveBuble())return;
            if (speechBubblePrefab != null && bubbleAnchor != null)
            {
                var bubble = Dependencies.GameObjectFactory.Instantiate(speechBubblePrefab, bubbleAnchor.position, Quaternion.identity, null);
                bubble.transform.SetParent(bubbleAnchor.transform);
                bubble.GetComponent<SpeechBubbleUI>().Show(text);
                Dependencies.EventBus.Raise(new SFXEvent { sfxData = audioData});
            }
        }

        public void ResetTalker()
        {
            nextSpeakDelay = Dependencies.RandomService.Range(minSpeakInterval, maxSpeakInterval);
            activeBubble = gameObject.GetComponentInChildren<SpeechBubbleUI>();
            if (activeBubble != null)
            {
                Dependencies.GameObjectFactory.Destroy(activeBubble.gameObject);
                activeBubble = null;
            }
        }

        private void ResetTimer()
        {
            elapsedTime = 0f;
            nextSpeakDelay = Dependencies.RandomService.Range(minSpeakInterval, maxSpeakInterval);
            //Debug.Log($"[FishTalker] Próximo diálogo en: {nextSpeakDelay} segundos.");
        }


        #region testing
        public bool IsValidConfiguration()
        {
            return speechBubblePrefab != null &&
                   bubbleAnchor != null &&
                   !string.IsNullOrEmpty(playerDialogueCsvPath) &&
                   !string.IsNullOrEmpty(npcDialogueCsvPath);
        }

        // Getter para testing (solo lectura)
        public bool HasActiveBubble => activeBubble != null;
        public int DialogueLineCount => dialogueLines?.Count ?? 0;
        #endregion
    }

}
