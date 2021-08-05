using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseSystem.Property;
using Handy;
using UniRx;
using UniRx.Triggers;
using System;
using Cysharp.Threading.Tasks;

public class Enemy : EnemyProp, IEnemy
{
    [Header("Parameter")]
    [SerializeField] int HP = 10;
    [SerializeField] int ATK = 2;
    [SerializeField] float m_speed;
    [SerializeField] Collider m_serchCollider;

    private GameObject m_target;
    private Rigidbody m_rigid;

    bool isMovePleyer = false;

    private void Awake()
    {
        hp = HP;
        atk = ATK;
        m_rigid = GetComponent<Rigidbody>();

        m_serchCollider.OnTriggerEnterAsObservable()
            .Where(other => other.gameObject.tag == "Player")
            .Subscribe(other => 
            {
                m_target = other.gameObject;
                isMovePleyer = true;

             });

        m_serchCollider.OnTriggerExitAsObservable()
        .Where(other => other.gameObject.tag == "Player")
        .Subscribe(other =>
        {
            m_target = other.gameObject;
            isMovePleyer = true;

        });
    }



    private void Update()
    {
        if (!isMovePleyer)
        {
            Tracer.trace(m_target.transform, this.transform, m_rigid, m_speed);
        }
        else
        {
            Tracer.trace(m_target.transform, this.transform, m_rigid, 1, m_speed);
        }
    }

    public void SetTarget(GameObject tower)
    {
        m_target = tower;
    }

    public void Damage(int attack)
    {
        hp -= attack;
    }

    public void OnAtkChanged(int value)
    {
        throw new System.NotImplementedException();
    }

    public void OnHpChanged(int value)
    {
        if (hp <= 0) Destroy(this);
    }
}
