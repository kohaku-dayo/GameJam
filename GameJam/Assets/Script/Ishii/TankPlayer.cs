using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseSystem.Property;

public class TankPlayer : BaseProp ,IPlayerProp
{
    private bool m_AttackStart = false;
    private float m_time;
    [SerializeField] float m_intarval;
    public float Cost => 5f;
    public bool IsDead { get; private set; }

    IEnemy target_IEnemy;

    private void Awake()
    {
        hp = 100;
        atk = 5;
    }

    private void Update()
    {
        Debug.Log(hp);
        if (m_AttackStart == true)
        {
            m_time += Time.deltaTime;
        }
        if (m_time > m_intarval)
        {
            // ‚±‚±‚ÉEnemy‚Éƒ_ƒ[ƒW‚ğ—^‚¦‚éˆ—
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
        if (value <= 0) Destroy(this.gameObject);
    }
}
