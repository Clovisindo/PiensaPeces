using Assets.Scripts.Core;
using System;
using UnityEngine;

namespace Assets.Scripts.Fish.NPC
{
    [CreateAssetMenu(fileName = "FishConfig", menuName = "Fish/Fish config", order = 1)]
    public class FishConfig: ScriptableObject
    {
        public Sprite fishSprite;
        public float speed = 1f;
        public float maxLifetime = 30f;
        [Range(0f, 1f)] public float swimProbability = 0.5f;
        public float intervalTalking;
        public float intervalEvaluateIntent;
        public AudioEmitterData sftTalk;
        //public DialogueProfile dialogueProfile;// si queremos expandir el dialogo


        public void Init()
        {
            maxLifetime = maxLifetime * Global.Instance.GAME_SPEED;
            intervalEvaluateIntent = UnityEngine.Random.Range(5f, 10f) * Global.Instance.GAME_SPEED;
            intervalTalking = UnityEngine.Random.Range(10f, 25f) * Global.Instance.GAME_SPEED;
        }
    }
}
