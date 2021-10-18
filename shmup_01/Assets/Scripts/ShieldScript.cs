using DG.Tweening;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class ShieldScript : MonoBehaviour
    {
        public SpriteRenderer SR;
        public Collider2D BC;

        private Sequence hitSequence;
        private Material defaultMaterial;

        public void Awake()
        {
            defaultMaterial = SR.material;
            hitSequence = DOTween.Sequence();
            hitSequence.Pause();
            hitSequence.SetAutoKill(false);
            hitSequence.AppendCallback(() => { SR.material = configScript.HitMaterial; });
            hitSequence.AppendInterval(0.1f);
            hitSequence.AppendCallback(() => { SR.material = defaultMaterial; });
        }

        public void Show()
        {
            SR.enabled = true;
        }

        public void Hide()
        {
            SR.enabled = false;
        }

        public bool CollideTest(Collider2D otherCollider, Bullet bullet)
        {
            if (otherCollider == BC && bullet.OwnerType == Bullet.BulletOwnerType.Player)
            {
                hitSequence.Restart();
                return true;
            }

            return false;
        }

        [Inject] private ConfigScript configScript;
    }
}