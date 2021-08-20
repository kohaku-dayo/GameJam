using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UniRx.Triggers;
using UniRx;
using System.Threading;


public class PlayerSord : PlayerAbstract
{
    [SerializeField] private Rigidbody m_rigidbody = default;
    [SerializeField] private float m_speed = default;
    [SerializeField] private Collider m_characterCollider = default;
    private bool m_isStop = false;

    public override void InitializePlayer(IManager manager)
    {
        base.InitializePlayer(manager);

        m_audioManager.PlaySE(6);

        m_hp
        .SkipLatestValueOnSubscribe()
        .Where(hp => hp <= 0)
        .Subscribe(_ => m_audioManager.PlaySE(8));

        m_characterCollider.OnTriggerEnterAsObservable()
            .Where(other => other.gameObject.tag == "Enemy")
            .Subscribe(_ => m_isStop = true)
            .AddTo(this);
    }

    protected override void AttackIntervalExcute()
    {
        if (m_isStop && m_target != null)
        {
            m_audioManager.PlaySE(7);
            m_anim.SetBool("Attack", true);
            DelayAttack(this.GetCancellationTokenOnDestroy()).Forget();
        }
    }

    protected override void AttckUpdateExcute()
    {
        if (m_target != null)
        {
            if (m_isStop)
            {
                m_rigidbody.velocity = Vector3.zero;
                m_anim.SetBool("Walk", false);
            }
            else
            {
                m_anim.SetBool("Walk", true);
                m_rigidbody.velocity = (m_target.transform.position - transform.position).normalized * m_speed;
            }
        }
        else if (m_target == null)
        {
            m_isStop = false;
            SetTarget(m_manager.EnemyList);
        }
    }

    private async UniTask DelayAttack(CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f),false,PlayerLoopTiming.Update, cancellationToken);
        if(m_target != null)
        {
            m_target.GetComponent<IDamage>().Damage(m_attack.Value);
            m_anim?.SetBool("Attack", false);
        }
    }
}
