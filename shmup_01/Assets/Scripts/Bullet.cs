using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class Bullet : MonoBehaviour
    {
        public enum BulletOwner
        {
            Player,
            Boss
        }

        private Rigidbody2D rigidBody;

        public MonoMemoryPool<Bullet> pool;

        public BulletOwner Owner;
        public float power = 10f;

        [Inject]
        public void Construct()
        {
            rigidBody = GetComponent<Rigidbody2D>();
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
            // Debug.Log("COLLISION!");
            if (_eyeController.CollideTest(otherCollider, this))
            {
                pool.Despawn(this);
            }

            if (_shipController.CollideTest(otherCollider, this))
            {
                pool.Despawn(this);
            }

            foreach (var turret in turretsController.GetAliveTurrets())
            {
                if (turret.CollideTest(otherCollider, this))
                {
                    pool.Despawn(this);
                }
            }
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

        [Inject] private EyeController _eyeController;
        [Inject] private ShipController _shipController;
        [Inject] private TurretsController turretsController;
    }
}