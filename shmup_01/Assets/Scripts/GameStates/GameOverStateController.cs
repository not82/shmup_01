using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace DefaultNamespace.GameStates
{
    public class GameOverStateController : BaseGameStateController, IInitializable, ITickable
    {
        public GameState GameState = GameState.GameOver;

        public new void Initialize()
        {
            gameOverRT.gameObject.SetActive(false);
        }

        public new void Tick()
        {
            if (_bossController.hp <= 0f)
            {
                _gameStateController.SetState(this.GameState);
            }

            if (IsActive)
            {
                if (Gamepad.current.aButton.wasPressedThisFrame)
                {
                    _gameStateController.SetState(GameState.InGame);
                }
            }
        }

        public override void OnEnter()
        {
            gameOverRT.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            gameOverRT.gameObject.SetActive(false);
        }

        [Inject(Id = "UI/GameOver")] private RectTransform gameOverRT;
        [Inject] private BossController _bossController;
        [Inject] private GameStateController _gameStateController;
    }
}