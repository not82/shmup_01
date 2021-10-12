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

    public void Initialize()
    {
        currentActionMode = ActionMode.Weapon;
        // Debug.Log(string.Join("\n", Gamepad.all));
    }

    public void Reset()
    {
        hp = maxHp;
        energy = startingEnergy;
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
            return true;
        }

        if (otherCollider == _shieldCollider && bullet.Owner == Bullet.BulletOwner.Boss)
        {
            energy += absorbEnergy;
            energy = Math.Min(maxEnergy, energy);
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

    [Inject(Id = "Ship")] private Transform transform;
    [Inject(Id = "Ship")] private BoxCollider2D _boxCollider2D;

    [Inject(Id = "Ship/Shield")] private BoxCollider2D _shieldCollider;

    [Inject(Id = "Ship/Weapon")] private Transform bulletOriginTransform;

    [Inject] private Bullet.Pool bulletFactory;
    [Inject] private ShmupSettings _shmupSettings;

    [Inject(Id = "Ship/RotationPoint")] private Transform rotationPoint;
}