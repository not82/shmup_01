using DefaultNamespace.GameStates;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.BossStates
{
    /*
     * Boss mort
     */
    public class BossStateDead : BaseGameStateController, IInitializable, ITickable
    {
        public void Initialize()
        {
        }

        public void Tick()
        {
            if (!IsActive)
            {
                return;
            }
        }

        public override void OnEnter()
        {
            Debug.Log("BOSS DEAD ENTER !");
        }

        public override void OnExit()
        {
            Debug.Log("BOSS DEAD EXIT !");
        }
    }
}