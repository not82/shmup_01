using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class LifesScript : MonoBehaviour
    {
        public Image[] lifesImage;

        // Value 1 to 3
        public void SetValue(int value)
        {
            for (var i = 0; i <= 2; i++)
            {
                lifesImage[i].enabled = (i >= 3 - value);
            }
        }
    }
}