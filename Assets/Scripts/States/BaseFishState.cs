using UnityEngine;

namespace Game.States
{
    public class BaseFishState : MonoBehaviour
    {
        public void Enter()
        {
            Debug.Log("Entering base fish state.");
        }


        public void Exit()
        {
            Debug.Log("Exiting base fish state.");
        }
    }
}
