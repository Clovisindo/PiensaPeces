using System.Collections.Generic;

namespace Assets.Scripts.Services.Enviroment
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "GroundEnvironmentDayConfig", menuName = "Environment/Ground Day Config")]
    public class GroundEnvironmentDayConfig : ScriptableObject
    {
        public int dayNumber;
        public List<GroundEnvConfig> groundEnvConfigs;
    }

}
