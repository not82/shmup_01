using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace.GameStates
{
    public class GameOverStateController : BaseGameStateController, IInitializable, ITickable
    {
        public new void Initialize()
        {
            gameOverRT.gameObject.SetActive(false);
        }

        public new void Tick()
        {
            if (IsActive)
            {
                // var elapsedTime = Time.realtimeSinceStartup - enterTime;
                if (Gamepad.current.startButton.wasPressedThisFrame)
                {
                    _gameStateController.SetState(GameState.InGame);
                }
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            gameOverRT.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            gameOverRT.gameObject.SetActive(false);
        }

        [Inject(Id = "UI/GameOver")] private RectTransform gameOverRT;

        [Inject] private GameStateController _gameStateController;
    }
}