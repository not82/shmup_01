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
    private Material defaultMaterial;

    public enum BehaviorPhase
    {
        Opened,
        Closed
    }

    private BehaviorPhase currentPhase;

    public void Initialize()
    {
        currentPhase = BehaviorPhase.Opened;
        defaultMaterial = _spriteRenderer.material;
        hitSequence = DOTween.Sequence();
        hitSequence.Pause();
        hitSequence.SetAutoKill(false);
        hitSequence.AppendCallback(() => { _spriteRenderer.material = _configScript.HitMaterial; });
        hitSequence.AppendInterval(0.1f);
        hitSequence.AppendCallback(() => { _spriteRenderer.material = defaultMaterial; });
    }

    public void Tick()
    {
    }

    public void Show()
    {
        _spriteRenderer.enabled = true;
        _boxCollider2D.enabled = true;
    }

    public void Hide()
    {
        _spriteRenderer.enabled = false;
        _boxCollider2D.enabled = false;
    }

    /**
     * On a recu un projectile
     */
    public bool CollideTest(Collider2D collider, Bullet bullet)
    {
        if (collider == _boxCollider2D && bullet.OwnerType == Bullet.BulletOwnerType.Player)
        {
            // Debug.Log("Eye shot !");
            // _spriteRenderer.color.DOColor(new Color(255f, 255f, 255f), 0.5f);
            // _spriteRenderer.color += new Color(40f,40f,40f);
            hitSequence.Restart();
            _bossController.hp -= bullet.power;
            return true;
        }

        return false;
    }

    [Inject] private BossController _bossController;

    [Inject(Id = "Boss/Eye")] private SpriteRenderer _spriteRenderer;
    [Inject(Id = "Boss/Eye")] private BoxCollider2D _boxCollider2D;

    [Inject] private ConfigScript _configScript;
}