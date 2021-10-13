using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LifesViewScript : MonoBehaviour
    {
        public SpriteRenderer[] lifesSR;

        // Value 1 to 3
        public void SetValue(int value)
        {
            for (var i = 0; i <= 2; i++)
            {
                lifesSR[i].enabled = (i >= 3 - value);
            }
        }
    }
}