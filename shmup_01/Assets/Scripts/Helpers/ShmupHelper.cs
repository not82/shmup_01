using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Helpers
{
    public class ShmupHelper
    {
        private List<Vector2> shipSavedPositions;

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

        public void SaveShipPositions()
        {
            shipSavedPositions = new List<Vector2>();
            foreach (var shipController in shipControllers)
            {
                shipSavedPositions.Add(new Vector2(shipController.transform.position.x,
                    shipController.transform.position.y));
            }
        }

        public void LoadShipPositions()
        {
            var i = 0;
            foreach (var shipController in shipControllers)
            {
                shipController.transform.position = new Vector3(shipSavedPositions[i].x, shipSavedPositions[i].y);
                i++;
            }
        }

        [Inject] private List<ShipController> shipControllers;
    }
}