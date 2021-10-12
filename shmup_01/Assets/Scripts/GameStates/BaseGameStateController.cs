using UnityEngine;
using Zenject;

namespace DefaultNamespace.GameStates
{
    public class BaseGameStateController : IGameStateController
    {
        public GameState GameState;
        public bool IsActive = false; 

        public virtual void OnEnter()
        {
            Debug.Log("BASE ENTER !");
        }

        public virtual void OnExit()
        {
        }
    }
}