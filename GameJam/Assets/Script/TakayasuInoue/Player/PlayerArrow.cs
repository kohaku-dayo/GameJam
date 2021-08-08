using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

/// <summary>
/// キャスターと特に振る舞いが変わらないのでキャスターにもアタッチ
/// </summary>
public class PlayerArrow : PlayerAbstract
{
    [SerializeField] GameObject m_bullet;
    protected override void AttackIntervalExcute()
    {
        if (m_target == null) return;
        m_anim.SetBool("Attack", true);
        InstantiateBullet(this.GetCancellationTokenOnDestroy()).Forget();
    }

    protected override void AttckUpdateExcute()
    {
        if(m_target == null)
        {
            SetTarget(m_manager.EnemyList);
        }
    }

    private async UniTask InstantiateBullet(CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.3f), false, PlayerLoopTiming.Update, cancellationToken);

        var bullet = Instantiate(m_bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<IBullet>().Initialize(this.gameObject, m_target);
        m_anim.SetBool("Attack", false);
    }
}



