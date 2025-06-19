using Assets.Scripts.Fish.NPC;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services.Enviroment
{
    [CreateAssetMenu(fileName = "FishEnvironmentDayConfig", menuName = "Environment/Fish Day Config")]
    public class FishEnvDayConfig : ScriptableObject
    {
        public int dayNumber;
        public List<FishConfig> fishEnvDayConfigs;
    }
}
