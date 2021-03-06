using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class Bullet : MonoBehaviour
    {
        public enum BulletOwnerType
        {
            Player,
            Boss
        }

        private Rigidbody2D rigidBody;

        public MonoMemoryPool<Bullet> pool;

        // Pour distinguer les 2 joueurs
        public GameObject Owner;

        public BulletOwnerType OwnerType;
        public float power = 10f;

        [Inject]
        public void Construct()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            // Debug.Log(shipControllers);
        }

        public void Update()
        {
            if (Position.y > 10f)
            {
                pool.Despawn(this);
            }

            if (Position.y < -10f)
            {
                pool.Despawn(this);
            }
        }

        public Vector3 Velocity
        {
            get { return rigidBody.velocity; }
            set { rigidBody.velocity = value; }
        }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            eyeControllers.ForEach(eyeController =>
            {
                if (eyeController.CollideTest(otherCollider, this))
                {
                    pool.Despawn(this);
                }
            });


            shipControllers.ForEach(shipController =>
            {
                if (shipController.CollideTest(otherCollider, this))
                {
                    pool.Despawn(this);
                }
            });

            foreach (var turret in turretsController.GetAliveTurrets())
            {
                if (turret.CollideTest(otherCollider, this))
                {
                    pool.Despawn(this);
                }
            }

            shieldScripts.ForEach(shieldScript =>
            {
                if (shieldScript.CollideTest(otherCollider, this))
                {
                    // Renvoi de la bullet
                    if (shieldScript.Reflect)
                    {
                        OwnerType = BulletOwnerType.Boss;
                        Velocity = -Velocity / 2f;
                    }
                    else
                    {
                        pool.Despawn(this);
                    }
                }
            });
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
        }

        public class Pool : MonoMemoryPool<Bullet>
        {
            protected override void OnSpawned(Bullet item)
            {
                item.pool = this;
                base.OnSpawned(item);
            }
        }

        public class Pool2 : MonoMemoryPool<Bullet>
        {
            protected override void OnSpawned(Bullet item)
            {
                item.pool = this;
                base.OnSpawned(item);
            }
        }

        [Inject] private List<EyeController> eyeControllers;
        [Inject] private List<ShipController> shipControllers;
        [Inject] private TurretsController turretsController;
        [Inject] private List<ShieldScript> shieldScripts;
    }
}