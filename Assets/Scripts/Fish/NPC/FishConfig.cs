using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            intervalEvaluateIntent = UnityEngine.Random.Range(5f, 10f);
            intervalTalking = UnityEngine.Random.Range(10f, 25f);
        }
    }
}
