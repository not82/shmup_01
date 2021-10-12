using System;
using System.Collections.Generic;
using DefaultNamespace;
using RedMagic.Zenject;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindController<ShipController>();
        Container.BindController<ModifierController>();
        Container.BindController<ModifierSettings>();
        
        // Container.BindFactory<Bullet, BulletFactory>()
        //     .FromPoolableMemoryPool(poolBinder => poolBinder
        //         .WithInitialSize(20)
        //         .FromComponentInNewPrefab(BulletPrefab)
        //         .WithGameObjectName("Bullet")
        //         .UnderTransformGroup("Bullets"));

        Container.BindMemoryPool<Bullet, Bullet.Pool>()
            .WithInitialSize(20)
            .FromComponentInNewPrefab(Laser1Prefab);
        
        Container.BindMemoryPool<Bullet, Bullet.Pool2>()
            .WithInitialSize(20)
            .FromComponentInNewPrefab(Laser2Prefab);
    }

    public GameObject Laser1Prefab;
    public GameObject Laser2Prefab;
    
}