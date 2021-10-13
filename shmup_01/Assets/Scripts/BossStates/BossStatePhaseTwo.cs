using DefaultNamespace.GameStates;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.BossStates
{
    /*
     * 2 tourelles du haut qui tirent en visant
     * Oeil caché
     */
    public class BossStatePhaseTwo : BaseGameStateController, IInitializable, ITickable
    {
        private float fireDelay = 0.2f; // In seconds
        private float lastBulletTime;

        public void Initialize()
        {
            // throw new System.NotImplementedException();
        }

        public void Tick()
        {
            if (!IsActive)
            {
                return;
            }

            var turrets = turretsController.GetAliveTurrets();

            if (turrets.Count > 0)
            {
                if (Time.realtimeSinceStartup > lastBulletTime + fireDelay)
                {
                    foreach (var turret in turrets)
                    {
                        turretsController.FireAimed(turret);
                    }

                    lastBulletTime = Time.realtimeSinceStartup;
                }
            }
            else
            {
                // Debug.Log("PHASE 2 All turrets dead!");
                // Phase 2
                // bossStateController.SetState(BossState.Phase2);
            }
        }

        public override void OnEnter()
        {
            Debug.Log("BOSS STATE2 ENTER !");
            turretsController.turretTopLeft.Show();
            turretsController.turretTopRight.Show();
            turretsController.turretBottomLeft.Hide();
            turretsController.turretBottomRight.Hide();
            eyeController.Show();

            lastBulletTime = Time.realtimeSinceStartup;
        }

        public override void OnExit()
        {
            Debug.Log("BOSS STATE2 EXIT !");
        }

        [Inject] private TurretsController turretsController;
        [Inject] private EyeController eyeController;
    }
}