using DG.Tweening;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class ShieldScript : MonoBehaviour
    {
        public SpriteRenderer SR;
        public Collider2D BC;

        public bool Reflect = false;

        public float Hp = 100;
        public float MaxHp = 100;

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
            Hp = MaxHp;
            SR.enabled = true;
            BC.enabled = true;
        }

        public void Hide()
        {
            SR.enabled = false;
            BC.enabled = false;
        }

        public bool CollideTest(Collider2D otherCollider, Bullet bullet)
        {
            if (otherCollider == BC && bullet.OwnerType == Bullet.BulletOwnerType.Player)
            {
                Hp -= bullet.power;
                hitSequence.Restart();
                if (Hp <= 0)
                {
                    Kill();
                }
                return true;
            }

            return false;
        }

        public void Kill()
        {
            Hide();
        }
        
        [Inject] private ConfigScript configScript;
    }
}