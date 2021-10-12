using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class ShipController : IInitializable, ITickable
{
    public void Initialize()
    {
        Debug.Log(string.Join("\n", Gamepad.all));
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
        Debug.Log("FIRE!");

        modifierSettings.ModifierSlots.ForEach(slot =>
        {
            if (slot.Modifier != null)
            {
                slot.Modifier.Fire(slot.ShipSiteTransform);
            }
        });

        // var bullet = bulletFactory.Spawn();

        // bullet.Position = new Vector3(bulletOriginTransform.position.x, bulletOriginTransform.position.y);
        // bullet.Velocity = new Vector3(0f, 10f);
    }

    [Inject(Id = "Ship/Transform")] private Transform transform;

    [Inject(Id = "Ship/BulletOrigin/Transform")]
    private Transform bulletOriginTransform;

    [Inject] private Bullet.Pool bulletFactory;
    [Inject] private Bullet.Pool2 bulletFactory2;
    [Inject] private ModifierSettings modifierSettings;
}