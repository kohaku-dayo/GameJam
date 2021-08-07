using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public interface IPlayerParameter
{
    IReadOnlyReactiveProperty<float> Hp { get; }
    IReadOnlyReactiveProperty<float> Cost { get; }
    IReadOnlyReactiveProperty<float> Attack { get; }
}

public abstract class PlayerAbstract : MonoBehaviour, IPlayerParameter, IDamage
{
    public IReadOnlyReactiveProperty<float> Hp => m_hp;
    public IReadOnlyReactiveProperty<float> Cost => m_cost;
    public IReadOnlyReactiveProperty<float> Attack => m_attack;
    [SerializeField] protected float m_attackInterval = default;
    [SerializeField] protected ReactiveProperty<float> m_hp = new ReactiveProperty<float>();
    [SerializeField] protected ReactiveProperty<float> m_cost = new ReactiveProperty<float>();
    [SerializeField] protected ReactiveProperty<float> m_attack = new ReactiveProperty<float>();

    protected GameObject m_targer;
    [SerializeField] Collider m_seerchCollider = default;
    [SerializeField] protected Animator m_anim = default;
    [SerializeField] Slider m_slider = default;
    float m_maxHp;

    protected BattleManager m_battleManager;
    protected abstract void AttackIntervalExcute();
    protected abstract void AttckUpdateExcute();

    protected virtual void Awake()
    {
        m_maxHp = m_hp.Value;

        //m_seerchCollider.OnTriggerEnterAsObservable()
        // .Where(other => other.gameObject.tag == "Enemy" && m_targer == null)
        // .Subscribe(other => m_targer = other.gameObject)
        // .AddTo(this);

        m_battleManager = GameObject.FindObjectOfType<BattleManager>();

        SetTarget(m_battleManager.EnemyList);

        AttackAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    public void SetTarget(List<GameObject> target)
    {
        float distance = Vector3.Distance(this.transform.position, target[0].transform.position);
        m_targer = target[0];
        foreach (var t in target)
        {
            var d = Vector3.Distance(this.transform.position, t.transform.position);
            if( d < distance)
            {
                distance = d;
                m_targer = t;
            }
        }

    }

    /// <summary>
    /// インターバル間隔でアタックを実行する
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    async UniTask AttackAsync(CancellationToken cancellationToken)
    {
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
        
            AttckUpdateExcute();

            if (time > m_attackInterval)
            {
                AttackIntervalExcute();
                time = 0;
            }
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
        }
    }

    public void Damage(float attack)
    {
        m_hp.Value -= attack;
        m_slider.value = m_hp.Value / m_maxHp;
        if (m_hp.Value <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}

