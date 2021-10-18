using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.BossStates;
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

    public Vector3 basePosition;

    private float spawnTime; // Time when boss as spawned

    private BehaviorPhase currentPhase;

    public void Initialize()
    {
        currentPhase = BehaviorPhase.Idle;
        basePosition = transform.position;
    }

    public void Reset()
    {
        hp = maxHp;
        spawnTime = Time.realtimeSinceStartup;
        bossStateController.SetState(BossState.Phase1);
    }

    public void Tick()
    {
        // var dx = 0;
        // var dy = 0;
        // var speed = 0.01f;

        var dt = Time.realtimeSinceStartup - spawnTime;
        var amplitude = 1f;
        var dx = Mathf.Sin(dt) + 0.5f * Mathf.Sin(dt * 2f);
        var dy = Mathf.Cos(dt) + 0.5f * Mathf.Cos(dt * 2f);

        transform.position = new Vector3(basePosition.x + dx * amplitude, basePosition.y + dy * amplitude);
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

    public float GetPercentHP()
    {
        return hp / maxHp * 100f;
    }

    [Inject(Id = "Boss")] private Transform transform;


    [Inject] private Bullet.Pool bulletFactory;
    [Inject] private Bullet.Pool2 bulletFactory2;
    [Inject] private ShmupSettings _shmupSettings;

    [Inject] private BossStateController bossStateController;
}