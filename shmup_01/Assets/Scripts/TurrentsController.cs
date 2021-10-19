using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class TurretsController : IInitializable, ITickable
{
    private float fireDelay = 1f; // In seconds
    private float bulletSpeed = 5f;
    private float lastBulletTime;

    public TurretScript turretTopLeft;
    public TurretScript turretTopRight;
    public TurretScript turretBottomLeft;
    public TurretScript turretBottomRight;

    public List<TurretScript> turretList = new List<TurretScript>();

    public void Initialize()
    {
        foreach (var turretTransform in turrets)
        {
            turretList.Add(turretTransform);
        }

        turretTopLeft = turrets[0];
        turretTopRight = turrets[1];
        turretBottomLeft = turrets[2];
        turretBottomRight = turrets[3];

        Reset();
    }

    public void Reset()
    {
        lastBulletTime = Time.realtimeSinceStartup;
    }

    public void Tick()
    {
    }

    public void Fire(TurretScript turret)
    {
        // Debug.Log("TURRET FIRE !");
        // foreach (var turret in turrets)
        // {
        var position = turret.transform.position;
        var bullet = bulletFactory.Spawn();
        bullet.OwnerType = Bullet.BulletOwnerType.Boss;
        bullet.Position = new Vector3(position.x, position.y);
        bullet.Velocity = new Vector3(0f, -bulletSpeed * turret.BulletOrientation);
        // }
    }

    public void FireConic(TurretScript turret, int bulletNumber, float speed, float coneAngle = -1f,
        Vector3? direction = null)
    {
        var position = turret.transform.position;

        var minAngle = -90f;
        var maxAngle = 90f;

        if (coneAngle != -1f)
        {
            minAngle = -coneAngle / 2f;
            maxAngle = coneAngle / 2f;
        }

        var withDirection = true;
        float aimedAngle;
        if (direction == null)
        {
            withDirection = false;
            direction = new Vector3();
            aimedAngle = 0;
        }
        else
        {
            aimedAngle = Mathf.Atan2(direction.Value.y, direction.Value.x) * Mathf.Rad2Deg;
        }

        // turret.GunTranform.rotation = Quaternion.Euler(0, 0, aimedAngle + 90);

        for (var iBullet = 0; iBullet < bulletNumber; iBullet++)
        {
            var bullet = bulletFactory.Spawn();
            bullet.OwnerType = Bullet.BulletOwnerType.Boss;
            bullet.Position = new Vector3(position.x, position.y);

            float angle;
            if (bulletNumber == 1)
            {
                angle = 0;
            }
            else
            {
                angle = Mathf.Lerp(minAngle, maxAngle, iBullet / (float) (bulletNumber - 1));
            }

            angle += aimedAngle;

            var dx = Mathf.Cos(angle * Mathf.Deg2Rad);
            var dy = Mathf.Sin(angle * Mathf.Deg2Rad);

            // bullet.Velocity = new Vector3(-bulletSpeed * dx, -bulletSpeed * turret.BulletOrientation);
            bullet.Velocity = new Vector3(
                -speed * dx * (withDirection ? -1 : turret.BulletOrientation),
                -speed * dy * (withDirection ? -1 : turret.BulletOrientation));

            var realAngle = Mathf.Atan2(bullet.Velocity.y, bullet.Velocity.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.AngleAxis(realAngle - 90, Vector3.forward);


            // bullet.Orientation = new Vector3(0f, 0f, angle * turret.BulletOrientation);
            // bullet.transform.rotation = Quaternion.LookRotation(bullet.Velocity);
        }
    }

    public void FireAimed(TurretScript turret)
    {
        var shipPosition = shipControllers[0].GetPosition();
        // foreach (var turret in turrets)
        // {
        var position = turret.transform.position;
        var direction = shipPosition - position;
        direction = direction.normalized;
        var bullet = bulletFactory.Spawn();
        bullet.OwnerType = Bullet.BulletOwnerType.Boss;
        bullet.Position = new Vector3(position.x, position.y);
        bullet.Velocity = direction * bulletSpeed;
        // }

        // direction = PlayerObject.transform.position - transform.position;
        // direction = direction.normalized;
    }

    public List<TurretScript> GetAliveTurrets()
    {
        return turretList.FindAll(turret => { return turret.Hp > 0; });
    }


    [Inject(Id = "Boss/Turrents")] private TurretScript[] turrets;
    [Inject] private Bullet.Pool2 bulletFactory;
    [Inject] private List<ShipController> shipControllers;
}