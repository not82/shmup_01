using UnityEngine;
using Zenject;

namespace DefaultNamespace.GameStates
{
    public class BaseGameStateController : IGameStateController
    {
        protected float enterTime;
        public bool IsActive = false;

        public virtual void OnEnter()
        {
            enterTime = Time.realtimeSinceStartup;
            Debug.Log("BASE ENTER !");
        }

        public virtual void OnExit()
        {
        }
    }
}