using System;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class Laser1Modifier : Modifier, IInitializable
    {
        [Inject] private Bullet.Pool bulletFactory;
        public void Initialize()
        {
            RenderPrefab = (GameObject) Resources.Load("Prefabs/Bullet", typeof(GameObject));
        }

        public override void Fire(Transform SiteTransform)
        {
            Debug.Log("LASER1!");

            var bullet = bulletFactory.Spawn();

            bullet.Position = new Vector3(SiteTransform.position.x, SiteTransform.position.y);
            bullet.Velocity = new Vector3(0f, 10f);
        }
    }
}