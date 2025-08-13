using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "AudioEnvironmentDayConfig", menuName = "Environment/Audio Day Config")]
    public class AudioEnvDayConfig : EnvDayConfigBase
    {
        //public int dayNumber;
        public List<AudioEnvConfig> audioConfigs;
    }
}
