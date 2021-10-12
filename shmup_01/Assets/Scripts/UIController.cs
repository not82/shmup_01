using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class UIController : IInitializable, ITickable
{

    public void Initialize()
    {
        
    }

    public void Tick()
    {
        // TODO Events
        _bossHP.text = _bossController.GetPercentHP().ToString() + '%';
    }

    private void fire()
    {
        // // Debug.Log("FIRE!");
        //
        // if (currentActionMode == ActionMode.Weapon)
        // {
        //     var bullet = bulletFactory.Spawn();
        //     bullet.Position = new Vector3(bulletOriginTransform.position.x, bulletOriginTransform.position.y);
        //     bullet.Velocity = new Vector3(0f, 10f);
        // }
    }

    [Inject(Id = "UI/BossHP")] private Text _bossHP;
    [Inject] private BossController _bossController;

}