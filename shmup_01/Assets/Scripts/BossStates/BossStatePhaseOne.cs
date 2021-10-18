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
        private float fireDelay = 0.5f; // In seconds
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

            if (
                bossController.GetBottomLink().CurrentState == BossTurrentEyeLinkScript.State.EyeDead
                && bossController.GetTopLink().CurrentState == BossTurrentEyeLinkScript.State.EyeDead
            )
            {
                bossStateController.SetState(BossState.Dead);
            }

            var turrets = turretsController.GetAliveTurrets();
            if (turrets.Count > 0)
            {
                if (Time.realtimeSinceStartup > lastBulletTime + fireDelay)
                {
                    foreach (var turret in turrets)
                    {
                        turretsController.FireConic(turret, 10, 2f);
                    }

                    lastBulletTime = Time.realtimeSinceStartup;
                }
            }
        }

        public override void OnEnter()
        {
            Debug.Log("BOSS STATE1 ENTER !");

            bossController.GetTopLink().TurretAliveEnter();
            bossController.GetBottomLink().TurretAliveEnter();

            // Show only bottom turrets
            // turretsController.turretTopLeft.Hide();
            // turretsController.turretTopRight.Hide();
            // turretsController.turretBottomLeft.Show();
            // turretsController.turretBottomRight.Show();
            // eyeController.Hide();

            lastBulletTime = Time.realtimeSinceStartup;
        }

        public override void OnExit()
        {
            Debug.Log("BOSS STATE1 EXIT !");
        }

        [Inject] private TurretsController turretsController;
        [Inject] private BossStateController bossStateController;

        [Inject] private BossController bossController;
        // [Inject] private EyeController eyeController;
    }
}