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
    private Sequence hitSequence;

    public enum BehaviorPhase
    {
        Opened,
        Closed
    }

    private BehaviorPhase currentPhase;

    public void Initialize()
    {
        currentPhase = BehaviorPhase.Opened;

        var baseColor = _spriteRenderer.color;
        hitSequence = DOTween.Sequence();
        hitSequence.Pause();
        hitSequence.SetAutoKill(false);
        hitSequence.Append(_spriteRenderer.DOColor(new Color(1f, 1f, 1f), 0.1f));
        hitSequence.Append(_spriteRenderer.DOColor(baseColor, 0.1f));
    }

    public void Tick()
    {
    }

    /**
     * On a recu un projectile
     */
    public bool CollideTest(Collider2D collider, Bullet bullet)
    {
        if (collider == _boxCollider2D && bullet.Owner == Bullet.BulletOwner.Player)
        {
            // Debug.Log("Eye shot !");
            // _spriteRenderer.color.DOColor(new Color(255f, 255f, 255f), 0.5f);
            // _spriteRenderer.color += new Color(40f,40f,40f);
            hitSequence.Restart();
            hitSequence.Play();
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