using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "FishEnvironmentDayConfig", menuName = "Environment/Fish Day Config")]
    public class FishEnvDayConfig : ScriptableObject
    {
        public int dayNumber;
        public List<FishConfig> fishEnvDayConfigs;
    }
}
