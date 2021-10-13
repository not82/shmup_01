using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class ShipController : IInitializable, ITickable
{
    public float speed = 0.02f;
    
    public float maxHp = 30f;
    public float hp = 30f;

    public float maxEnergy = 100f;
    public float startingEnergy = 50f;
    public float energy;

    public float fireEnergyCost = 10f;
    public float absorbEnergy = 10f;

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
    
    public void Initialize()
    {
        currentActionMode = ActionMode.Weapon;
        // Debug.Log(string.Join("\n", Gamepad.all));

        circleSequence = DOTween.Sequence();
        circleSequence.Pause();
        circleSequence.SetAutoKill(false);
        circleSequence.AppendInterval(5f);
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
        circleSequence.Play();
    }

    public void Tick()
    {
        var dx = 0;
        var dy = 0;

        // Debug.Log(transform.position);
        if (Gamepad.current.aButton.wasPressedThisFrame)
        {
            fire();
        }

        if (Gamepad.current.bButton.wasPressedThisFrame)
        {
            switchWeaponShieldHandler();
        }

        if (Gamepad.current.leftStick.left.isPressed)
        {
            dx = -1;
        }

        if (Gamepad.current.leftStick.right.isPressed)
        {
            dx = 1;
        }

        if (Gamepad.current.leftStick.up.isPressed)
        {
            dy = 1;
        }

        if (Gamepad.current.leftStick.down.isPressed)
        {
            dy = -1;
        }

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
                bullet.Owner = Bullet.BulletOwner.Player;
                bullet.Position = new Vector3(bulletOriginTransform.position.x, bulletOriginTransform.position.y);
                bullet.Velocity = new Vector3(0f, 10f);
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
                rotationPoint.DOLocalRotate(new Vector3(0, 0, 180), 0.5f);
                currentActionMode = ActionMode.Shield;
                break;
            case ActionMode.Shield:
                rotationPoint.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
                currentActionMode = ActionMode.Weapon;
                break;
        }
    }

    public bool CollideTest(Collider2D otherCollider, Bullet bullet)
    {
        if (otherCollider == _boxCollider2D && bullet.Owner == Bullet.BulletOwner.Boss)
        {
            hp -= bullet.power;
            hitSequence.Restart();
            return true;
        }

        if (otherCollider == _shieldCollider && bullet.Owner == Bullet.BulletOwner.Boss)
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
    
    public float GetPercentEnergy()
    {
        return energy / maxEnergy * 100f;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    [Inject(Id = "Ship")] private Transform transform;
    [Inject(Id = "Ship")] private BoxCollider2D _boxCollider2D;
    [Inject(Id = "Ship")] private SpriteRenderer _hullSR;
    
    [Inject(Id = "Ship/Shield")] private BoxCollider2D _shieldCollider;
    [Inject(Id = "Ship/Shield")] private SpriteRenderer _shieldSR;
    
    [Inject(Id = "Ship/Circle")] private SpriteRenderer _circleSR;
   

    [Inject(Id = "Ship/Weapon")] private Transform bulletOriginTransform;

    
    [Inject] private Bullet.Pool bulletFactory;
    [Inject] private ShmupSettings _shmupSettings;

    [Inject(Id = "Ship/RotationPoint")] private Transform rotationPoint;

    [Inject] private ConfigScript _configScript;
}