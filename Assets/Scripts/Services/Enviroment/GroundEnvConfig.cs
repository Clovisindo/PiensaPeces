using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Services.Enviroment
{
    [CreateAssetMenu(fileName = "GroundEnvConfig", menuName = "Ground/ground config", order = 1)]
    public class GroundEnvConfig: ScriptableObject
    {
        public GameObject prefab;
        public Sprite sprite;
    }
}
