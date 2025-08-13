using System.Collections.Generic;

namespace Game.Data
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "GroundEnvironmentDayConfig", menuName = "Environment/Ground Day Config")]
    public class GroundEnvironmentDayConfig : EnvDayConfigBase
    {
        //public int dayNumber;
        public List<GroundEnvConfig> groundEnvConfigs;
    }

}
