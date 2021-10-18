using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class EyeController : MonoBehaviour, IInitializable
{
    private Sequence hitSequence;
    private Material defaultMaterial;

    public float MaxHp = 100f;
    public float Hp = 100f;

    public enum BehaviorPhase
    {
        Opened,
        Closed
    }

    private BehaviorPhase currentPhase;

    public void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        currentPhase = BehaviorPhase.Opened;
        defaultMaterial = SR.material;
        hitSequence = DOTween.Sequence();
        hitSequence.Pause();
        hitSequence.SetAutoKill(false);
        hitSequence.AppendCallback(() => { SR.material = _configScript.HitMaterial; });
        hitSequence.AppendInterval(0.1f);
        hitSequence.AppendCallback(() => { SR.material = defaultMaterial; });
    }

    public void Show()
    {
        SR.enabled = true;
        _boxCollider2D.enabled = true;
    }

    public void Hide()
    {
        SR.enabled = false;
        _boxCollider2D.enabled = false;
    }

    public void Kill()
    {
        SR.enabled = false;
    }

    /**
     * On a recu un projectile
     */
    public bool CollideTest(Collider2D collider, Bullet bullet)
    {
        if (collider == _boxCollider2D && bullet.OwnerType == Bullet.BulletOwnerType.Player)
        {
            Hp -= bullet.power;
            hitSequence.Restart();
            if (Hp <= 0)
            {
                Kill();
            }

            return true;
        }

        return false;
    }


    public SpriteRenderer SR;
    public BoxCollider2D _boxCollider2D;

    // [Inject] private BossController _bossController;

    [Inject] private ConfigScript _configScript;
}