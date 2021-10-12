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

    public void Tick()
    {
        var dx = 0;
        var dy = 0;

        var speed = 0.01f;

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
            var bullet = bulletFactory.Spawn();
            bullet.Position = new Vector3(bulletOriginTransform.position.x, bulletOriginTransform.position.y);
            bullet.Velocity = new Vector3(0f, 10f);
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

    [Inject(Id = "Ship")] private Transform transform;

    [Inject(Id = "Ship/Weapon")] private Transform bulletOriginTransform;

    [Inject] private Bullet.Pool bulletFactory;
    [Inject] private Bullet.Pool2 bulletFactory2;
    [Inject] private ShmupSettings _shmupSettings;

    [Inject(Id = "Ship/RotationPoint")] private Transform rotationPoint;
}