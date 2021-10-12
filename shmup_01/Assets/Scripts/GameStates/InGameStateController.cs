using UnityEngine;
using Zenject;

namespace DefaultNamespace.GameStates
{
    public class InGameStateController : BaseGameStateController, IInitializable, ITickable
    {
        public new void Initialize()
        {
            // gameOverRT.gameObject.SetActive(false);
        }

        public new void Tick()
        {
            // if (_bossController.hp <= 0f)
            // {
            //     _gameController.SetState(GameController.GameStates.GameOver);
            //     OnEnter();
            // }
        }

        public override void OnEnter()
        {
            // Debug.Log("IN GAME ENTER !");
            _bossController.Reset();
        }

        public override void OnExit()
        {
            // Debug.Log("IN GAME EXIT !");
        }

        [Inject] private GameStateController _gameStateController;
        [Inject] private BossController _bossController;
    }
}