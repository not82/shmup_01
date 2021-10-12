using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class BossController : IInitializable, ITickable
{
    public enum BehaviorPhase
    {
        Idle,
        Shooting
    }

    public float maxHp = 100f;
    public float hp = 100f;

    private BehaviorPhase currentPhase;

    public void Initialize()
    {
        currentPhase = BehaviorPhase.Idle;
    }

    public void Reset()
    {
        hp = maxHp;
    }

    public void Tick()
    {
        var dx = 0;
        var dy = 0;

        var speed = 0.01f;

        transform.position = new Vector3(transform.position.x + dx * speed, transform.position.y + dy * speed);
    }

    private void fire()
    {
        // // Debug.Log("FIRE!");
        //
        // if (currentActionMode == ActionMode.Weapon)
        // {
        //     var bullet = bulletFactory.Spawn();
        //     bullet.Position = new Vector3(bulletOriginTransform.position.x, bulletOriginTransform.position.y);
        //     bullet.Velocity = new Vector3(0f, 10f);
        // }
    }


    [Inject(Id = "Boss")] private Transform transform;

    [Inject(Id = "Ship/Weapon")] private Transform bulletOriginTransform;


    [Inject] private Bullet.Pool bulletFactory;
    [Inject] private Bullet.Pool2 bulletFactory2;
    [Inject] private ShmupSettings _shmupSettings;

    [Inject(Id = "Ship/RotationPoint")] private Transform rotationPoint;

    public float GetPercentHP()
    {
        return hp / maxHp * 100f;
    }
}