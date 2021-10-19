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

    public float BulletSpeed = 10f;
    public float BulletOrientation = 1f;

    public bool VerticalLocked = false;

    public bool AutoFire = true;
    public float AutoFireSpeed = 0.2f;

    public float RespawnUntouchableTime = 5f;
    private float lastRespawnTime;
    private Vector2 respawnPosition;

    public LifesScript LifesScript;

    public enum State
    {
        Dead,
        Respawning,
        Alive
    }

    private State currentState;

    private Sequence circleSequence;

    private Sequence hitSequence;
    private Sequence blinkSequence;
    private Material defaultMaterial;

    private Sequence shieldHitSequence;

    private float dx;
    private float dy;

    private float lastFireTime = -1f;

    private Gamepad gamepad;

    private Vector2 screenBounds;

    public State CurrentState => currentState;

    public void Awake()
    {
        Initialize();
        respawnPosition = new Vector2(transform.position.x, transform.position.y);
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
        if (gamepadIndex >= Gamepad.all.Count)
        {
            // No gamepad => disable this ship
            deadStart();
            return;
        }

        screenBounds =
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        gamepad = Gamepad.all[gamepadIndex];

        defaultMaterial = _hullSR.material;
        hitSequence = DOTween.Sequence();
        hitSequence.Pause();
        hitSequence.SetAutoKill(false);
        hitSequence.AppendCallback(() => { _hullSR.material = _configScript.HitMaterial; });
        hitSequence.AppendInterval(0.1f);
        hitSequence.AppendCallback(() => { _hullSR.material = defaultMaterial; });

        blinkSequence = DOTween.Sequence();
        blinkSequence.Pause();
        blinkSequence.SetAutoKill(false);
        blinkSequence.SetLoops(-1);
        blinkSequence.AppendCallback(() => { _hullSR.enabled = false; });
        blinkSequence.AppendInterval(0.1f);
        blinkSequence.AppendCallback(() => { _hullSR.enabled = true; });
        blinkSequence.AppendInterval(0.1f);
    }

    public void Reset()
    {
        hp = maxHp;
        LifesScript.SetValue(hp - 1);
        energy = startingEnergy;
        lastFireTime = Time.realtimeSinceStartup;
        aliveStart();
    }

    public void Tick()
    {
        dx = 0;
        dy = 0;

        if (AutoFire)
        {
            if (gamepad.aButton.isPressed)
            {
                tryToFire();
            }
        }
        else
        {
            if (gamepad.aButton.wasPressedThisFrame)
            {
                fire();
            }
        }

        if (gamepad.bButton.wasPressedThisFrame)
        {
            blinkSequence.Restart();
        }

        if (gamepad.leftStick.left.isPressed)
        {
            dx = -1;
        }

        if (gamepad.leftStick.right.isPressed)
        {
            dx = 1;
        }

        if (!VerticalLocked)
        {
            if (gamepad.leftStick.up.isPressed)
            {
                dy = 1;
            }

            if (gamepad.leftStick.down.isPressed)
            {
                dy = -1;
            }
        }

        if (CurrentState == State.Respawning)
        {
            if (Time.realtimeSinceStartup > lastRespawnTime + RespawnUntouchableTime)
            {
                respawnOver();
            }
        }
    }

    public void FixedTick()
    {
        var margin = 0.2f;
        transform.position =
            new Vector3(
                Mathf.Clamp(transform.position.x + dx * speed, -screenBounds.x + margin, screenBounds.x - margin),
                Mathf.Clamp(transform.position.y + dy * speed, -screenBounds.y + margin, screenBounds.y + margin));
    }

    private void tryToFire()
    {
        if (Time.realtimeSinceStartup > lastFireTime + AutoFireSpeed)
        {
            fire();
        }
    }

    private void fire()
    {
        if (energy >= fireEnergyCost)
        {
            var bullet = bulletFactory.Spawn();
            bullet.OwnerType = Bullet.BulletOwnerType.Player;
            bullet.Owner = gameObject;
            bullet.Position = new Vector3(bulletOriginTransform.position.x, bulletOriginTransform.position.y);
            bullet.Velocity = new Vector3(0f, BulletOrientation * BulletSpeed);
            energy -= fireEnergyCost;

            lastFireTime = Time.realtimeSinceStartup;
        }
        else
        {
            // TODO
        }
    }

    public bool CollideTest(Collider2D otherCollider, Bullet bullet)
    {
        // Hit by boss
        if (otherCollider == _boxCollider2D && bullet.OwnerType == Bullet.BulletOwnerType.Boss)
        {
            manageHit();
            return true;
        }

        // Hit by player
        if (otherCollider == _boxCollider2D && bullet.OwnerType == Bullet.BulletOwnerType.Player &&
            bullet.Owner != gameObject)
        {
            manageHit();
            return true;
        }

        return false;
    }

    private void manageHit()
    {
        hp -= 1;
        hp = Math.Max(0, hp);
        if (hp > 0)
        {
            LifesScript.SetValue(hp - 1);
            respawnStart();
        }
        else
        {
            deadStart();
            // TODO Gameover
        }
    }

    public float GetPercentEnergy()
    {
        return energy / maxEnergy * 100f;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void respawnStart()
    {
        currentState = State.Respawning;
        lastRespawnTime = Time.realtimeSinceStartup;
        _boxCollider2D.enabled = false;
        transform.position = new Vector3(respawnPosition.x, respawnPosition.y);
        blinkSequence.Restart();
    }

    public void respawnOver()
    {
        blinkSequence.Pause();
        _hullSR.enabled = true;
        _boxCollider2D.enabled = true;
        currentState = State.Alive;
    }

    public void deadStart()
    {
        gameObject.SetActive(false);
        currentState = State.Dead;
    }

    public void aliveStart()
    {
        respawnOver();
        gameObject.SetActive(true);
        currentState = State.Alive;
    }

    public Transform transform;
    public BoxCollider2D _boxCollider2D;
    public SpriteRenderer _hullSR;
    public Transform bulletOriginTransform;

    [Inject] private Bullet.Pool bulletFactory;

    [Inject] private ConfigScript _configScript;
}