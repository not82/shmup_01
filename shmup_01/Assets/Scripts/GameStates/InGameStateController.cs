using System.Collections.Generic;
using DefaultNamespace.BossStates;
using UnityEngine;
using UnityEngine.InputSystem;
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
                if (bossStateController.GetCurrentState() == BossState.Dead)
                {
                    gameStateController.SetState(GameState.Success);
                }


                shipControllers.ForEach(shipController =>
                {
                    if (shipController.hp <= 0f)
                    {
                        gameStateController.SetState(GameState.GameOver);
                    }
                });
                
                if (Gamepad.current.startButton.wasPressedThisFrame)
                {
                    gameStateController.SetState(GameState.InGame);
                }
            }
        }

        public override void OnEnter()
        {
            // Debug.Log("IN GAME ENTER !");
            gameController.Reset();
            shipControllers.ForEach(shipController => { shipController.Reset(); });
            bossController.Reset();
            turretsController.Reset();
        }

        public override void OnExit()
        {
            // Debug.Log("IN GAME EXIT !");
        }

        [Inject] private GameStateController gameStateController;
        [Inject] private List<ShipController> shipControllers;
        [Inject] private BossController bossController;
        [Inject] private BossStateController bossStateController;
        [Inject] private TurretsController turretsController;
        [Inject] private GameController gameController;
    }
}