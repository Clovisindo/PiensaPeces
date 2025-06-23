using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services.Enviroment
{
    [CreateAssetMenu(fileName = "AudioEnvironmentDayConfig", menuName = "Environment/Audio Day Config")]
    public class AudioEnvDayConfig : ScriptableObject
    {
        public int dayNumber;
        public List<AudioEnvConfig> audioConfigs;
    }
}
