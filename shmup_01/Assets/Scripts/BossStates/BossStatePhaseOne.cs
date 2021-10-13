using DefaultNamespace.GameStates;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.BossStates
{
    /*
     * 2 tourelles du bas qui tirent devant elles
     * Oeil caché
     */
    public class BossStatePhaseOne : BaseGameStateController, IInitializable, ITickable
    {
        private float fireDelay = 1f; // In seconds
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
                        turretsController.Fire(turret);
                    }

                    lastBulletTime = Time.realtimeSinceStartup;
                }
            }
            else
            {
                // Phase 2
                bossStateController.SetState(BossState.Phase2);
            }
        }

        public override void OnEnter()
        {
            Debug.Log("BOSS STATE1 ENTER !");
            // Show only bottom turrets
            turretsController.turretTopLeft.Hide();
            turretsController.turretTopRight.Hide();
            turretsController.turretBottomLeft.Show();
            turretsController.turretBottomRight.Show();
            eyeController.Hide();
            
            lastBulletTime = Time.realtimeSinceStartup;
        }

        public override void OnExit()
        {
            Debug.Log("BOSS STATE1 EXIT !");
        }

        [Inject] private TurretsController turretsController;
        [Inject] private BossStateController bossStateController;
        [Inject] private EyeController eyeController;
    }
}