using UnityEngine;

namespace Game.Utilities
{
    public class Global : MonoBehaviour
    {
        public static Global Instance;

        //[SerializeField] public readonly float GAME_SPEED = 1.0f;
        [SerializeField] public readonly float GAME_SPEED = 30.0f;

        private void Awake()
        {
            if ( Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
