using System.Collections.Generic;
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
            if (IsActive)
            {
                if (_bossController.hp <= 0f)
                {
                    _gameStateController.SetState(GameState.Success);
                }

                // TODO GAMEOVER ? Revive ? ( si l'autre tir sur la dépouille ? )
                // if (_shipControllers.hp <= 0f)
                // {
                //     _gameStateController.SetState(GameState.GameOver);
                // }
            }
        }

        public override void OnEnter()
        {
            // Debug.Log("IN GAME ENTER !");
            gameController.Reset();
            _shipControllers.ForEach(ship => ship.Reset());
            _bossController.Reset();
            _turretsController.Reset();
        }

        public override void OnExit()
        {
            // Debug.Log("IN GAME EXIT !");
        }

        [Inject] private GameStateController _gameStateController;
        [Inject] private List<ShipController> _shipControllers;
        [Inject] private BossController _bossController;
        [Inject] private TurretsController _turretsController;
        [Inject] private GameController gameController;
    }
}