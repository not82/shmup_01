using System;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class Laser2Modifier : Modifier
    {
        [Inject] private Bullet.Pool2 bulletFactory;
        public override void Fire(Transform SiteTransform)
        {
            Debug.Log("LASER2!");
            var bullet = bulletFactory.Spawn();

            bullet.Position = new Vector3(SiteTransform.position.x, SiteTransform.position.y);
            bullet.Velocity = new Vector3(0f, 10f);
        }
    }
}