using System;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.GameStates;
using RedMagic.Zenject;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindController<ShipController>();
        Container.BindController<BossController>();
        Container.BindController<EyeController>();

        Container.BindController<UIController>();

        Container.BindController<ShmupSettings>();

        Container.BindController<GameStateController>();
        Container.BindController<InGameStateController>();
        Container.BindController<GameOverStateController>();


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