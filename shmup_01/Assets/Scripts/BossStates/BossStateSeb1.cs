using System.Collections.Generic;
using DefaultNamespace.GameStates;
using DefaultNamespace.Helpers;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.BossStates
{
    /*
     * 2 tourelles du bas qui tirent devant elles
     * Oeil caché
     */
    public class BossStateSeb1 : BaseGameStateController, IInitializable, ITickable
    {
        // private float fireDelay = 2f; // In seconds
        private float lastBulletTime;

        private List<float> currentSalve;
        private int currentSalveIndexInSalve;
        private List<List<float>> salveDefinitons = new List<List<float>>();

        public void Initialize()
        {
            // salveDefinitons = new List<List<float>>(4);
            salveDefinitons.Add(new List<float> {2f});
            salveDefinitons.Add(new List<float> {2f, 0.2f});
            salveDefinitons.Add(new List<float> {2f, 0.2f, 0.2f});
            salveDefinitons.Add(new List<float> {2f, 0.2f, 0.2f, 0.2f});

            // throw new System.NotImplementedException();
        }

        public void Tick()
        {
            if (!IsActive)
            {
                return;
            }

            var turrets = turretsController.GetAliveTurrets();

            if (turrets.Count == 0)
            {
                bossStateController.SetState(BossState.Dead);
                return;
            }
            
            currentSalve = salveDefinitons[4 - turrets.Count];

            foreach (var turret in turrets)
            {
                // Turrents target is the nearest player
                turret.CurrentTarget = shmupHelper.getNearestPlayerShip(turret.transform.position);
                // turret.CurrentTarget = shipControllers[0];
            }

            if (Time.realtimeSinceStartup > lastBulletTime + currentSalve[currentSalveIndexInSalve])
            {
                foreach (var turret in turrets)
                {
                    var direction = (Vector2) turret.CurrentTarget.transform.position -
                                    (Vector2) turret.transform.position;
                    // direction = direction.normalized;
                    turretsController.FireConic(turret, 3, 3f, 30f, direction);
                }

                // 
                lastBulletTime = Time.realtimeSinceStartup;
                currentSalveIndexInSalve++;
                if (currentSalveIndexInSalve >= currentSalve.Count)
                {
                    currentSalveIndexInSalve = 0;
                }
            }

            if (turrets.Count > 0)
            {
                // if (Time.realtimeSinceStartup > lastBulletTime + fireDelay)
                // {
                //     foreach (var turret in turrets)
                //     {
                //         turretsController.FireConic(turret, 10, 2f);
                //     }
                //
                //     lastBulletTime = Time.realtimeSinceStartup;
                // }
            }
        }

        public override void OnEnter()
        {
            Debug.Log("BOSS STATESEB1 ENTER !");
            turretsController.turretTopLeft.Show();
            turretsController.turretTopRight.Show();
            turretsController.turretBottomLeft.Show();
            turretsController.turretBottomRight.Show();

            currentSalve = salveDefinitons[0];
            currentSalveIndexInSalve = 0;

            lastBulletTime = Time.realtimeSinceStartup;
        }

        public override void OnExit()
        {
            Debug.Log("BOSS STATESEB1 EXIT !");
        }

        [Inject] private ShmupHelper shmupHelper;

        [Inject] private TurretsController turretsController;
        [Inject] private BossStateController bossStateController;

        [Inject] private BossController bossController;
        // [Inject] private EyeController eyeController;

        [Inject] private List<ShipController> shipControllers;
    }
}