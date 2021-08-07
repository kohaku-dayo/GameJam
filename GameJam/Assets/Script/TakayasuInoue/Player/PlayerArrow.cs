using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;


public class PlayerArrow : PlayerAbstract
{
    [SerializeField] GameObject m_bullet;
    protected override void AttackIntervalExcute()
    {
        if (m_targer == null) return;
        m_anim.SetBool("Attack", true);
        InstantiateBullet(this.GetCancellationTokenOnDestroy()).Forget();
    }

    protected override void AttckUpdateExcute()
    {
        
    }

    private async UniTask InstantiateBullet(CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.3f), false, PlayerLoopTiming.Update, cancellationToken);

        var bullet = Instantiate(m_bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<IBullet>().Initialize(this.gameObject, m_targer);
        m_anim.SetBool("Attack", false);
    }
}



