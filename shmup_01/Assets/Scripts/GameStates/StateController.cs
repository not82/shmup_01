using System.Collections.Generic;
using Zenject;

namespace DefaultNamespace.GameStates
{
    // T => Enum de state
    public class StateController<T> where T : struct 
    {
        private T currentState;
        private BaseGameStateController currentStateController;
        private Dictionary<T, BaseGameStateController> states = new Dictionary<T, BaseGameStateController>();
        
        public void SetState(T state)
        {
            if (currentStateController != null)
            {
                currentStateController.IsActive = false;
                currentStateController.OnExit();
            }

            currentState = state;
            BaseGameStateController newStateController;
            states.TryGetValue(state, out newStateController);

            if (newStateController != null)
            {
                currentStateController = newStateController;
            }

            currentStateController.IsActive = true;
            currentStateController.OnEnter();
        }

        public T GetCurrentState()
        {
            return currentState;
        }

        public BaseGameStateController GetCurrentStateController()
        {
            return currentStateController;
        }

        public void AddState(T state, BaseGameStateController stateController)
        {
            states.Add(state, stateController);
        }
    }
}