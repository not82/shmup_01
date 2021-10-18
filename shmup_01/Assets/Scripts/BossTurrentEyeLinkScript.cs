using System;
using DefaultNamespace.GameStates;
using UnityEngine;

namespace DefaultNamespace
{
    public class BossTurrentEyeLinkScript : MonoBehaviour
    {
        public State CurrentState;

        public enum State
        {
            TurretAlive,
            TurretDead,
            EyeDead,
        }

        public void Awake()
        {
        }

        public void TurretAliveEnter()
        {
            CurrentState = State.TurretAlive;
            Turret.Show();
            Eye.Hide();
        }

        public void TurretDeadEnter()
        {
            CurrentState = State.TurretDead;
            Turret.Hide();
            Eye.Show();
        }

        public void EyeDeadEnter()
        {
            Debug.Log("EyeDeadEnter");
            CurrentState = State.EyeDead;
            Turret.Hide();
            Eye.Hide();
        }

        public void Update()
        {
            switch (CurrentState)
            {
                case State.TurretAlive:
                    if (Turret.Hp <= 0)
                    {
                        TurretDeadEnter();
                    }

                    break;
                case State.TurretDead:
                    if (Eye.Hp <= 0)
                    {
                        EyeDeadEnter();
                    }

                    break;
            }
        }

        private StateController<State> stateController;

        public TurretScript Turret;
        public EyeController Eye;
    }
}