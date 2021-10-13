using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnergyViewScript : MonoBehaviour
    {
        public Transform MaskTransform;

        public Transform MaxValueTransform;
        public Transform MinValueTransform;
        
        // Value 0 to 1
        public void SetPercentValue(float value)
        {
            value = value / 100f;
            MaskTransform.position = Vector3.Lerp(MinValueTransform.position, MaxValueTransform.position, value);
        }
    }
}