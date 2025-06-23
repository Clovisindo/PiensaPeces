using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Services.Enviroment
{
    [CreateAssetMenu(fileName = "FoodEnvironmentDayConfig", menuName = "Environment/Food Day Config")]
    public class FoodEnvDayConfig : ScriptableObject
    {
        public int dayNumber;
        public List<FoodEnvConfig> foodEnvConfigs;
    }
}
