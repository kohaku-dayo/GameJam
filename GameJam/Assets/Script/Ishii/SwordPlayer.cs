using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseSystem.Property;

public class SwordPlayer : BaseProp, IPlayerProp
{
    private bool m_AttackStart = false;
    private float m_time;
    [SerializeField] float m_intarval;
    public float Cost => 8;

    IEnemy target_IEnemy;

    private void Awake()
    {
        hp = 50;
        atk = 20;
    }

    private void Update()
    {
        if (m_AttackStart == true)
        {
            m_time += Time.deltaTime;
        }
        if (m_time > m_intarval)
        {
            // Ç±Ç±Ç…EnemyÇ…É_ÉÅÅ[ÉWÇó^Ç¶ÇÈèàóù
            target_IEnemy.Damage(atk);
            m_time = 0;
        }
    }

    public void Dmage(int atk)
    {
        hp -= atk;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            m_AttackStart = true;
            target_IEnemy = other.gameObject.GetComponent<IEnemy>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            m_AttackStart = false;
        }
    }

    public void OnAtkChanged(int value)
    {
        throw new System.NotImplementedException();
    }

    public void OnHpChanged(int value)
    {
        throw new System.NotImplementedException();
    }
}
