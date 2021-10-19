using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace.GameStates
{
    public class SuccessStateController : BaseGameStateController, IInitializable, ITickable
    {
        public new void Initialize()
        {
            successRT.gameObject.SetActive(false);
        }

        public new void Tick()
        {
            if (IsActive)
            {
                var elapsedTime = Time.realtimeSinceStartup - enterTime;
                if (Gamepad.current.startButton.wasPressedThisFrame && elapsedTime > 2f)
                {
                    _gameStateController.SetState(GameState.InGame);
                }
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            successRT.gameObject.SetActive(true);
            successText.text = "You defeated the boss in " + gameController.GetCurrentTime().ToString("F1") + "s";
        }

        public override void OnExit()
        {
            successRT.gameObject.SetActive(false);
        }

        [Inject(Id = "UI/Success")] private RectTransform successRT;
        [Inject(Id = "UI/Success/Subtext")] private Text successText;

        [Inject] private GameStateController _gameStateController;
        [Inject] private GameController gameController;
    }
}