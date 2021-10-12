using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody2D rigidBody;
        
        public MonoMemoryPool<Bullet> pool;
        
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
    }
}