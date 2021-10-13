using System;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class LifesViewScript : MonoBehaviour
    {
        public SpriteRenderer[] lifesSR;

        private Sequence showSequence;

        public void Awake()
        {
            showSequence = DOTween.Sequence();
            showSequence.Pause();
            showSequence.SetAutoKill(false);
            // showSequence.AppendInterval(2f);
            foreach (var lifeSR in lifesSR)
            {
                showSequence.Insert(2f, lifeSR.DOFade(0f, 2f));
            }
        }

        // Value 1 to 3
        public void SetValue(int value)
        {
            for (var i = 0; i <= 2; i++)
            {
                lifesSR[i].enabled = (i >= 3 - value);
            }
        }

        public void Show()
        {
            showSequence.Restart();
        }
    }
}