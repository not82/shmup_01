using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class EyeController : IInitializable, ITickable
{
    public enum BehaviorPhase
    {
        Opened,
        Closed
    }

    private BehaviorPhase currentPhase;

    public void Initialize()
    {
        currentPhase = BehaviorPhase.Opened;
    }

    public void Tick()
    {
    }

    /**
     * On a recu un projectile
     */
    public bool CollideTest(Collider2D collider, Bullet bullet)
    {
        if (collider == _boxCollider2D)
        {
            // Debug.Log("Eye shot !");
            _bossController.hp -= bullet.power;
            return true;
        }

        return false;
    }

    [Inject] private BossController _bossController;

    [Inject(Id = "Boss/Eye")] private SpriteRenderer _spriteRenderer;
    [Inject(Id = "Boss/Eye")] private BoxCollider2D _boxCollider2D;

    [Inject] private ShmupSettings _shmupSettings;
}