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
namespace Goriyasu
{
    public abstract class PlayerAbstract : MonoBehaviour, IPlayerParameter, IDamage
    {
        public IReadOnlyReactiveProperty<float> Hp => m_hp;
        public IReadOnlyReactiveProperty<float> Cost => m_cost;
        public IReadOnlyReactiveProperty<float> Attack => m_attack;
        [SerializeField] float m_attackInterval = default;
        [SerializeField] private ReactiveProperty<float> m_hp = new ReactiveProperty<float>();
        [SerializeField] private ReactiveProperty<float> m_cost = new ReactiveProperty<float>();
        [SerializeField] private ReactiveProperty<float> m_attack = new ReactiveProperty<float>();

        protected GameObject m_targer;
        [SerializeField] Collider m_seerchCollider = default;
        [SerializeField] protected Animator m_anim = default;
        [SerializeField] Slider m_slider = default;
        float m_maxHp;
        protected abstract void AttackExcute();

        private void Awake()
        {
            m_maxHp = m_hp.Value;

            m_seerchCollider.OnTriggerStayAsObservable()
                .Where(other => other.gameObject.tag == "Enemy")
                .Subscribe(other => m_targer = other.gameObject);
            AttackAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        public void SetTarget(GameObject target)
        {
            m_targer = target;
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
                m_anim.SetBool("Attack", false);
                if (time > m_attackInterval)
                {
                    AttackExcute();
                    time = 0;
                }
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
        }

        public void Damage(float attack)
        {
            m_hp.Value -= attack;
            m_slider.value = m_hp.Value / m_maxHp;
            if(m_hp.Value <= 0)
            {
                Destroy(this.gameObject);
            }
        }

    }
}
