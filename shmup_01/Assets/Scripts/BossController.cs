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

    public BehaviorPhase CurrentPhase;

    public void Initialize()
    {
        CurrentPhase = BehaviorPhase.Idle;
        basePosition = transform.position;
    }

    public void Reset()
    {
        hp = maxHp;
        spawnTime = Time.realtimeSinceStartup;
        bossStateController.SetState(BossState.Seb1);
    }

    public void Tick()
    {
        var dt = Time.realtimeSinceStartup - spawnTime;
        var amplitudeX = 0.5f;
        var amplitudeY = 0.2f;
        var dx = Mathf.Sin(dt) + 0.5f * Mathf.Sin(dt * 2f);
        var dy = Mathf.Cos(dt) + 0.5f * Mathf.Cos(dt * 2f);
        
        transform.position = new Vector3(basePosition.x + dx * amplitudeX, basePosition.y + dy * amplitudeY);
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

    public BossTurrentEyeLinkScript GetTopLink()
    {
        return TurrentEyeLinks[0];
    }

    public BossTurrentEyeLinkScript GetBottomLink()
    {
        return TurrentEyeLinks[1];
    }

    [Inject(Id = "Boss")] private Transform transform;


    [Inject] private Bullet.Pool bulletFactory;
    [Inject] private Bullet.Pool2 bulletFactory2;
    [Inject] private ShmupSettings _shmupSettings;

    [Inject] private BossStateController bossStateController;

    [Inject] public List<BossTurrentEyeLinkScript> TurrentEyeLinks;
}