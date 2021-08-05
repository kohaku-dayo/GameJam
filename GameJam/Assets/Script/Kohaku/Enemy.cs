using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseSystem.Property;
using UniRx;
using UniRx.Triggers;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class Enemy : EnemyProp, IEnemy
{
    [Header("Parameter")]
    [SerializeField] int HP = 10;
    [SerializeField] int ATK = 2;
    [SerializeField] float m_speed;
    [SerializeField] List<Collider> m_serchCollider;
    [SerializeField] Slider m_slider;

    private GameObject m_target;
    private GameObject m_Tower;
    private Rigidbody m_rigid;
    private Vector3 m_targetpos;
    bool isMovePleyer = false;
    bool iscollider = false;
    [SerializeField] float m_intarval = 3;
    Vector3 thispos;
    public int Attack{ get; set; }
    int m_maxHp;
    private void Awake()
    {
        hp = HP;
        m_maxHp = HP;
        Attack = ATK;
        m_rigid = GetComponent<Rigidbody>();
        m_Tower = GameObject.FindGameObjectWithTag("Tower");
        SetTrigger();
    }

    void SetTrigger()
    {
        foreach (var c in m_serchCollider)
        {
            c.OnTriggerEnterAsObservable()
                .Where(other => other.gameObject.tag == "Player")
                .Subscribe(other =>
                {
                    m_target = other.gameObject;
                    isMovePleyer = true;
                    thispos = this.transform.position;
                    m_targetpos = m_target.transform.position + (this.transform.position - m_target.transform.position).normalized;
                });

            c.OnTriggerExitAsObservable()
            .Where(other => other.gameObject.tag == "Player")
            .Subscribe(other =>
            {
                //m_target = other.gameObject;
                Debug.Log("Exit");
                isMovePleyer = false;

            });
        }
    }

    float attackTime;
   
    private void Update()
    {

        if (!isMovePleyer)
        {
            Debug.Log("ChaseTower");

            m_rigid.velocity = (m_Tower.transform.position - this.transform.position).normalized * m_speed;
            //transform.position += (m_targetpos - this.transform.position).normalized * m_speed * Time.deltaTime;

        }
        else
        {
            if (Vector3.Distance(m_target.transform.position, this.transform.position) < 127f)
            {
                m_rigid.velocity = Vector3.zero;
                Debug.Log("Stop");
                attackTime += Time.deltaTime;

                if (attackTime > m_intarval)
                {
                    // ここにEnemyにダメージを与える処理を書く
                    m_target.GetComponent<IPlayerProp>().Dmage(Attack);
                    
                    Destroy(m_target.gameObject);
                    isMovePleyer = false;

                    Debug.Log("Destroy");
                    attackTime = 0;
                }
                return;
            }
            //Debug.Log(Vector3.Distance(m_targetpos, this.transform.position));
            //Debug.Log("ChasePlayer");
            m_rigid.velocity = (m_target.transform.position - thispos);
           
        }
    }

    public void SetTarget(GameObject tower)
    {

    }

   

    public void Damage(int attack)
    {
        hp -= attack;
        //m_slider.value = hp / m_maxHp;
    }

    public void OnAtkChanged(int value)
    {
        throw new System.NotImplementedException();
    }

    public void OnHpChanged(int value)
    {
        Debug.Log(value);
        if (hp <= 0) Destroy(this.gameObject);
    }
    public  bool trace(Vector3 target, Vector3 self, Rigidbody selfrb, float traceSpeed = 1f)
    {
        //if (target)
        //{
        //    Debug.Log("Target");
        //}
        //else if (self)
        //{
        //    Debug.Log("SElf");
        //}
      
        var direction = (target - self).normalized;
        if (Vector3.Distance(target, self) < 0.1f)
        {
            selfrb.velocity = Vector3.zero;
            return false;
        }
        else selfrb.velocity = direction * traceSpeed;
        return true;
    }
    /// <summary>
    /// ターゲットとの範囲指定可能追跡機能
    /// </summary>
    public  bool trace2(GameObject target, GameObject self, Rigidbody selfrb, float DontMoveAreaRange, float traceSpeed = 1f)
    {
        var direction = (target.transform.position - self.transform.position).normalized;
        if (Vector3.Distance(target.transform.position, self.transform.position) < DontMoveAreaRange)
        {
            selfrb.velocity = Vector3.zero;
            return false;
        }
        else selfrb.velocity = direction * traceSpeed;
        return true;
    }
}
