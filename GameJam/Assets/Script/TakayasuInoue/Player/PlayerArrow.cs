using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace Goriyasu
{
    public class PlayerArrow : PlayerAbstract
    {
        [SerializeField] GameObject m_bullet;
        protected override void AttackExcute()
        {
            if (m_targer == null) return;
            m_anim.SetBool("Attack", true);
            InstantiateBullet().Forget();
        }

        private async UniTask InstantiateBullet()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            var bullet = Instantiate(m_bullet, transform.position, Quaternion.AngleAxis(90, Vector3.right));
            bullet.GetComponent<IBullet>().Initialize(this.gameObject, m_targer);
        }
    }


}
