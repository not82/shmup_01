using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class TimeScript : MonoBehaviour
    {
        public Text ValueText;
        
        public void SetValue(float value)
        {
            if (value == -1f)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                ValueText.text = value.ToString("#.#") + "s";
            }
        }
    }
}