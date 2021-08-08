using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseSystem.Property;
using UniRx;
using UniRx.Triggers;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public interface IEnemyParameter
{
    IReadOnlyReactiveProperty<float> Hp { get; }
    IReadOnlyReactiveProperty<float> Cost { get; }
    IReadOnlyReactiveProperty<float> Attack { get; }
}
public class Enemy : MonoBehaviour, IDamage,IEnemyParameter
{
    [SerializeField] float m_speed;
    [SerializeField] List<Collider> m_serchCollider;
    [SerializeField] Collider m_mineCollider;
    [SerializeField] Slider m_slider;
    [SerializeField] GameObject effect;
    [SerializeField] Animator m_anim;
    private GameObject m_target;
    private GameObject m_Tower;
    private Rigidbody m_rigid;
    [SerializeField] float m_intarval = 3;
    private bool m_isStop = false;
   
    public IReadOnlyReactiveProperty<float> Hp => m_hp;
    public IReadOnlyReactiveProperty<float> Cost => m_cost;
    public IReadOnlyReactiveProperty<float> Attack => m_attack;

    [SerializeField] private ReactiveProperty<float> m_hp = new ReactiveProperty<float>();
    [SerializeField] private ReactiveProperty<float> m_cost = new ReactiveProperty<float>();
    [SerializeField] private ReactiveProperty<float> m_attack = new ReactiveProperty<float>();

    private float m_maxHp;
    private IManager m_manager;
    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();

        m_maxHp = m_hp.Value;
 
        SetTrigger();

        m_anim.SetBool("Walk", true);
    }

    void SetTrigger()
    {
        m_mineCollider.OnTriggerEnterAsObservable()
                .Where(other => other.gameObject.tag == "PlayerCollider")
                .Subscribe(other =>
                {
                    m_target = other.gameObject.transform.parent.gameObject;
                })
                .AddTo(this);

        m_mineCollider.OnTriggerEnterAsObservable()
               .Where(other => other.gameObject.tag == "Tower")
               .Subscribe(other =>
               {
                   m_isStop = true;
               })
               .AddTo(this);
    }

    float attackTime;
   
    private void Update()
    {
        if (m_target == null)
        {
            if (m_isStop)
            {
                StopAndAttack(m_Tower);
                return;
            }
            m_rigid.velocity = (m_Tower.transform.position - this.transform.position).normalized * m_speed;
            MoveDirectionRotation(m_rigid.velocity.x);
        }
        else
        {
            StopAndAttack(m_target);
        }
    }

    private void MoveDirectionRotation(float x)
    {
        var rot = transform.rotation;

        rot.z = x > 0 ? 180 : 0;

        transform.rotation = rot;
    }

    void StopAndAttack(GameObject target)
    {
        m_rigid.velocity = Vector3.zero;

        attackTime += Time.deltaTime;

        if (attackTime > m_intarval)
        {
            target.GetComponent<IDamage>().Damage(m_attack.Value);
            attackTime = 0;
        }
    }

    public void Initialize(GameObject tower,IManager manager)
    {
        m_Tower = tower;
        m_manager = manager;
        manager.GameOver.Subscribe(_ => Destroy(this.gameObject)).AddTo(this);
    }

    public void Damage(float attack)
    {
        m_hp.Value -= attack;
        m_slider.value = m_hp.Value /m_maxHp;
        if (m_hp.Value <= 0) 
        {
            //m_manager.AddCost(m_Cost);　//敵が死んだときにコストを加えるなら
            Destroy(this.gameObject);
        }
    
        Effect().Forget();
    }
    async UniTask Effect()
    {
        var effects = Instantiate(effect, transform.position, Quaternion.AngleAxis(90, Vector3.right));
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        Destroy(effects);
    }

 
}
