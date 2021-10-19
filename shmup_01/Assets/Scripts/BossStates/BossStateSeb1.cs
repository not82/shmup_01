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
        // private float lastBulletTime;

        // private List<float> currentSalve;
        // private int currentSalveIndexInSalve;
        private List<List<float>> salveDefinitons = new List<List<float>>();

        public void Initialize()
        {
            shmupHelper.SaveShipPositions();
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

            foreach (var turret in turrets)
            {
                // Turrents target is the nearest player
                turret.CurrentTarget = shmupHelper.getNearestPlayerShip(turret.transform.position);
                if (turret.CurrentTarget != null)
                {
                    var direction = (Vector2) turret.CurrentTarget.transform.position -
                                    (Vector2) turret.transform.position;
                    var aimedAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    turret.GunTranform.rotation = Quaternion.Euler(0, 0, aimedAngle + 90);
                }
            }


            foreach (var turret in turrets)
            {
                if (turret.CurrentTarget != null)
                {
                    turret.CurrentSalve = salveDefinitons[4 - turrets.Count];
                    if (Time.realtimeSinceStartup >
                        turret.LastBulletTime + turret.CurrentSalve[turret.CurrentSalveIndexInSalve])
                    {
                        var direction = (Vector2) turret.CurrentTarget.transform.position -
                                        (Vector2) turret.transform.position;
                        // direction = direction.normalized;
                        turretsController.FireConic(turret, 3, 3f, 30f, direction);
                        turret.LastBulletTime = Time.realtimeSinceStartup;

                        turret.CurrentSalveIndexInSalve++;
                        if (turret.CurrentSalveIndexInSalve >= turret.CurrentSalve.Count)
                        {
                            turret.CurrentSalveIndexInSalve = 0;
                        }
                    }
                }
            }
        }

        public override void OnEnter()
        {
            Debug.Log("BOSS STATESEB1 ENTER !");
            shmupHelper.LoadShipPositions();

            turretsController.turretTopLeft.Show();
            turretsController.turretTopRight.Show();
            turretsController.turretTopRight.LastBulletTime += 0.2f;
            turretsController.turretBottomLeft.Show();
            turretsController.turretBottomLeft.LastBulletTime += 0.4f;
            turretsController.turretBottomRight.Show();
            turretsController.turretBottomRight.LastBulletTime += 0.6f;

            turretsController.turretList.ForEach(turret =>
            {
                turret.CurrentSalve = salveDefinitons[0];
                turret.CurrentSalveIndexInSalve = 0;
            });

            // lastBulletTime = Time.realtimeSinceStartup;
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