using DG.Tweening;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class TurretScript : MonoBehaviour
    {
        public float HpMax;
        public float Hp;

        public float BulletOrientation = 1f;

        public SpriteRenderer SR;
        public Transform Transform;
        public BoxCollider2D BC;
        public ShieldScript ShieldScript;

        public ShipController CurrentTarget;

        public Transform GunTranform;

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

        // public void Reset()
        // {
        //     Hp = HpMax;
        // }

        public void Show()
        {
            SR.enabled = true;
            Hp = HpMax;
        }

        public void Hide()
        {
            Hp = 0;
            SR.enabled = false;
            ShieldScript.Hide();
        }

        public void Kill()
        {
            Hide();
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

        [Inject] private ConfigScript configScript;
    }
}