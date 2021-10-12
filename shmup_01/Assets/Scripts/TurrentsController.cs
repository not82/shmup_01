using System;
using System.Collections;
using System.Collections.Generic;
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

    public enum BehaviorPhase
    {
        Idle,
        Shooting
    }

    private BehaviorPhase currentPhase;

    public void Initialize()
    {
        currentPhase = BehaviorPhase.Idle;
        Reset();
    }

    public void Reset()
    {
        lastBulletTime = Time.realtimeSinceStartup;
        // hp = maxHp;
    }

    public void Tick()
    {
        // var dx = 0;
        // var dy = 0;
        // var speed = 0.01f;
        // transform.position = new Vector3(transform.position.x + dx * speed, transform.position.y + dy * speed);
        if (Time.realtimeSinceStartup > lastBulletTime + fireDelay)
        {
            fire();
            lastBulletTime = Time.realtimeSinceStartup;
        }
    }

    private void fire()
    {
        Debug.Log("TURRENTS FIRE !");
        foreach (var turrentTransform in turrentTransforms)
        {
            var position = turrentTransform.position;
            var bullet = bulletFactory.Spawn();
            bullet.Owner = Bullet.BulletOwner.Boss;
            bullet.Position = new Vector3(position.x, position.y);
            bullet.Velocity = new Vector3(0f, -bulletSpeed);
        }
    }


    [Inject(Id = "Boss/Turrents")] private Transform[] turrentTransforms;
    [Inject] private Bullet.Pool2 bulletFactory;
}