using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class ShipController : MonoBehaviour, IInitializable, IFixedTickable, ITickable
{
    public int gamepadIndex;

    public float speed = 0.08f;

    public int maxHp = 3;
    public int hp = 3;

    public float maxEnergy = 100f;
    public float startingEnergy = 100f;
    public float energy;

    public float fireEnergyCost = 10f; // 10f
    public float absorbEnergy = 20f;

    public float bulletSpeed = 10f;
    public float bulletOrientation = 1f;

    public enum ActionMode
    {
        Weapon,
        Shield
    }

    private ActionMode currentActionMode;

    private Sequence circleSequence;

    private Sequence hitSequence;
    private Material defaultMaterial;

    private Sequence shieldHitSequence;

    private float dx;
    private float dy;

    private Gamepad gamepad;

    public void Awake()
    {
        Initialize();
    }

    public void Update()
    {
        Tick();
    }

    public void FixedUpdate()
    {
        FixedTick();
    }

    public void Initialize()
    {
        gamepad = Gamepad.all[gamepadIndex];

        currentActionMode = ActionMode.Weapon;
        // Debug.Log(string.Join("\n", Gamepad.all));

        circleSequence = DOTween.Sequence();
        circleSequence.Pause();
        circleSequence.SetAutoKill(false);
        circleSequence.AppendInterval(2f);
        circleSequence.Append(_circleSR.DOFade(0f, 2f));

        defaultMaterial = _hullSR.material;
        hitSequence = DOTween.Sequence();
        hitSequence.Pause();
        hitSequence.SetAutoKill(false);
        hitSequence.AppendCallback(() => { _hullSR.material = _configScript.HitMaterial; });
        hitSequence.AppendInterval(0.1f);
        hitSequence.AppendCallback(() => { _hullSR.material = defaultMaterial; });

        shieldHitSequence = DOTween.Sequence();
        shieldHitSequence.Pause();
        shieldHitSequence.SetAutoKill(false);
        shieldHitSequence.AppendCallback(() => { _shieldSR.material = _configScript.HitMaterial; });
        shieldHitSequence.AppendInterval(0.1f);
        shieldHitSequence.AppendCallback(() => { _shieldSR.material = defaultMaterial; });
    }

    public void Reset()
    {
        hp = maxHp;
        energy = startingEnergy;
        ShowCircle();
        // circleSequence.Play();
    }

    public void Tick()
    {
        dx = 0;
        dy = 0;

        // Debug.Log(transform.position);
        if (gamepad.aButton.wasPressedThisFrame)
        {
            fire();
        }

        if (gamepad.bButton.wasPressedThisFrame)
        {
            switchWeaponShieldHandler();
        }

        if (gamepad.leftStick.left.isPressed)
        {
            dx = -1;
        }

        if (gamepad.leftStick.right.isPressed)
        {
            dx = 1;
        }

        // if (gamepad.leftStick.up.isPressed)
        // {
        //     dy = 1;
        // }
        //
        // if (gamepad.leftStick.down.isPressed)
        // {
        //     dy = -1;
        // }
    }

    public void FixedTick()
    {
        transform.position = new Vector3(transform.position.x + dx * speed, transform.position.y + dy * speed);
    }

    private void fire()
    {
        // Debug.Log("FIRE!");

        if (currentActionMode == ActionMode.Weapon)
        {
            if (energy >= fireEnergyCost)
            {
                var bullet = bulletFactory.Spawn();
                bullet.OwnerType = Bullet.BulletOwnerType.Player;
                bullet.Owner = gameObject;
                bullet.Position = new Vector3(bulletOriginTransform.position.x, bulletOriginTransform.position.y);
                bullet.Velocity = new Vector3(0f, bulletOrientation * bulletSpeed);
                energy -= fireEnergyCost;
            }
            else
            {
                // TODO
            }
        }
    }

    private void switchWeaponShieldHandler()
    {
        Debug.Log("SWITCH!");
        switch (currentActionMode)
        {
            case ActionMode.Weapon:
                rotationPoint.DOLocalRotate(new Vector3(0, 0, 180), 0.2f);
                currentActionMode = ActionMode.Shield;
                break;
            case ActionMode.Shield:
                rotationPoint.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
                currentActionMode = ActionMode.Weapon;
                break;
        }
    }

    public bool CollideTest(Collider2D otherCollider, Bullet bullet)
    {
        if (otherCollider == _boxCollider2D && bullet.OwnerType == Bullet.BulletOwnerType.Boss)
        {
            hp -= 1;
            hitSequence.Restart();
            ShowCircle();
            return true;
        }

        if (otherCollider == _boxCollider2D && bullet.OwnerType == Bullet.BulletOwnerType.Player &&
            bullet.Owner != gameObject)
        {
            hp -= 1;
            hitSequence.Restart();
            ShowCircle();
            return true;
        }

        if (otherCollider == _shieldCollider && bullet.OwnerType == Bullet.BulletOwnerType.Boss)
        {
            energy += absorbEnergy;
            energy = Math.Min(maxEnergy, energy);
            shieldHitSequence.Restart();
            return true;
        }

        return false;
    }

    public float GetPercentHP()
    {
        return hp / maxHp * 100f;
    }

    public int GetHP()
    {
        return hp;
    }

    public float GetPercentEnergy()
    {
        return energy / maxEnergy * 100f;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void ShowCircle()
    {
        // circleSequence.Restart();
        // lifesViewScript.Show();
    }

    public Transform transform;
    public BoxCollider2D _boxCollider2D;
    public SpriteRenderer _hullSR;
    public BoxCollider2D _shieldCollider;
    public SpriteRenderer _shieldSR;
    public SpriteRenderer _circleSR;
    public Transform rotationPoint;
    public Transform bulletOriginTransform;
    public LifesViewScript lifesViewScript;

    [Inject] private Bullet.Pool bulletFactory;

    [Inject] private ConfigScript _configScript;
}