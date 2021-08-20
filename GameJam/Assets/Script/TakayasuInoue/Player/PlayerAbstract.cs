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
    /// ジェネレーター側から呼び出すプレイヤーの初期化。初期化処理を追加したければオーバーライドする
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
    /// 敵のリストから最も近い敵をターゲットに設定する
    /// </summary>
    /// <param name="target"></param>
    protected void SetTarget(List<GameObject> target)
    {
        //ターゲットが一人もいなかったらリターン
        if (target.Count == 0) return;
        Debug.Log("Set");
        //ターゲット初期設定
        float nearDistance = Vector3.Distance(this.transform.position, target[0].transform.position);
        m_target = target[0];

        //最短距離のエネミーを探索
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
    /// インターバル間隔でアタックを実行する
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private　async UniTask AttackAsync(CancellationToken cancellationToken)
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

