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
                CurrentTime.SetValue(gameController.GetCurrentTime());

                if (bossStateController.GetCurrentState() == BossState.Dead)
                {
                    gameStateController.SetState(GameState.Success);
                }

                var totalHp = 0;
                shipControllers.ForEach(shipController => { totalHp += shipController.hp; });
                if (totalHp <= 0f)
                {
                    gameStateController.SetState(GameState.GameOver);
                }

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
            base.OnEnter();
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
        [Inject(Id = "UI/CurrentTime")] private TimeScript CurrentTime;
    }
}