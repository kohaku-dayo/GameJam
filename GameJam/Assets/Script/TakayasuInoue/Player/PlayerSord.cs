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
    protected override void Awake()
    {
        base.Awake();

        m_characterCollider.OnTriggerEnterAsObservable()
            .Where(other => other.gameObject.tag == "Enemy")
            .Subscribe(_ => m_isStop = true)
            .AddTo(this);
    }

    protected override void AttackIntervalExcute()
    {
        if (m_isStop && m_targer != null)
        {
            m_anim.SetBool("Attack", true);
            DelayAttack(this.GetCancellationTokenOnDestroy()).Forget();
        }
    }

    protected override void AttckUpdateExcute()
    {
        if (!m_isStop && m_targer != null)
        {
            m_rigidbody.velocity = (m_targer.transform.position - transform.position).normalized * m_speed;
        }
        else if (m_isStop && m_targer == null)
        {
            SetTarget(m_battleManager.EnemyList);

            m_isStop = false;
        }
        else
        {
            m_rigidbody.velocity = Vector3.zero;
        }
    }

    private async UniTask DelayAttack(CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1),false,PlayerLoopTiming.Update, cancellationToken);
        m_targer?.GetComponent<IDamage>().Damage(m_attack.Value);
        m_anim?.SetBool("Attack", false);
    }
}
