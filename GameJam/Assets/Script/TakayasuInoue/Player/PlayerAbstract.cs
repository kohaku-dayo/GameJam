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

    protected GameObject m_target;
    protected AudioManager m_audioManager;
    [SerializeField] Collider m_seerchCollider = default;
    [SerializeField] protected Animator m_anim = default;
    [SerializeField] Slider m_slider = default;
    float m_maxHp;
    protected IManager m_manager;


    protected abstract void AttackIntervalExcute();
    protected abstract void AttckUpdateExcute();

    /// <summary>
    /// �W�F�l���[�^�[������Ăяo���v���C���[�̏������B������������ǉ���������΃I�[�o�[���C�h����
    /// </summary>
    /// <param name="manager"></param>
    public virtual void InitializePlayer(IManager manager)
    {
        m_manager = manager;
        m_maxHp = m_hp.Value;
        m_manager.GameOver.Subscribe(_ => Destroy(this.gameObject)).AddTo(this);
        SetTarget(m_manager.EnemyList);
        m_audioManager = FindObjectOfType<AudioManager>();
        AttackAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    /// <summary>
    /// �G�̃��X�g����ł��߂��G���^�[�Q�b�g�ɐݒ肷��
    /// </summary>
    /// <param name="target"></param>
    protected void SetTarget(List<GameObject> target)
    {
        //�^�[�Q�b�g����l�����Ȃ������烊�^�[��
        if (target.Count == 0) return;
        Debug.Log("Set");
        //�^�[�Q�b�g�����ݒ�
        float nearDistance = Vector3.Distance(this.transform.position, target[0].transform.position);
        m_target = target[0];

        //�ŒZ�����̃G�l�~�[��T��
        foreach (var t in target)
        {
            var distance = Vector3.Distance(this.transform.position, t.transform.position);
            if (distance < nearDistance)
            {
                nearDistance = distance;
                m_target = t;
            }
        }
    }

    /// <summary>
    /// �C���^�[�o���Ԋu�ŃA�^�b�N�����s����
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private�@async UniTask AttackAsync(CancellationToken cancellationToken)
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

