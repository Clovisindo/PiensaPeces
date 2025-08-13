using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "FoodEnvironmentDayConfig", menuName = "Environment/Food Day Config")]
    public class FoodEnvDayConfig : EnvDayConfigBase
    {
        //public int dayNumber;
        public List<FoodEnvConfig> foodEnvConfigs;
    }
}
