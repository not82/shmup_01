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
        _shipHP.text = _shipControllers[0].GetPercentHP().ToString() + '%';

        _shipEnergy.text = _shipControllers[0].GetPercentEnergy().ToString() + '%';

        // energyViewScript.SetPercentValue(_shipController.GetPercentEnergy());
        // lifesViewScript.SetValue(_shipController.GetHP());
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
    [Inject(Id = "UI/ShipHP")] private Text _shipHP;
    [Inject(Id = "UI/Energy")] private Text _shipEnergy;
    [Inject] private List<ShipController> _shipControllers;

    [Inject] private BossController _bossController;
    // [Inject] private EnergyViewScript energyViewScript;
    // [Inject] private LifesViewScript lifesViewScript;
}