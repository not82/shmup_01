using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class ShmupSettings : IInitializable
    {
        [Inject] private DiContainer _diContainer;

        public void Initialize()
        {
            // var laser1Modifier = _diContainer.Instantiate<Laser1Modifier>();
            // laser1Modifier.UISkill = laser1Skill;
            // Modifiers.Add(laser1Modifier);
        }
    }
}