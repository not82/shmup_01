using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Helpers
{
    public class ShmupHelper
    {
        public ShipController getNearestPlayerShip(Vector2 position)
        {
            float nearestDistance = 0f;
            ShipController nearestShip = null;
            foreach (var player in shipControllers)
            {
                var distance = Vector2.Distance(player.transform.position, position);
                if (nearestShip == null || nearestDistance > distance)
                {
                    nearestShip = player;
                    nearestDistance = distance;
                }
            }

            return nearestShip;
        }

        [Inject] private List<ShipController> shipControllers;
    }
}