using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseSystem.Property;

public class ArcherPlayer : BaseProp, IPlayerProp
{
    private bool m_AttackStart = false;
    private float m_time;
    [SerializeField] float m_intarval;
    public float Cost => 8;

    IEnemy target_IEnemy;
    [SerializeField] GameObject arrow;

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
            // ここにEnemyにダメージを与える処理
            // Enemyは矢を放つので、放った瞬間ダメージを与える処理は行いません。
            // 矢のスクリプトにてダメージ処理を行ってください。
            // 矢へArcherのatkを遷移するので、ArcherスクリプトにてIarrowDamageインターフェースを実装してください。
            GameObject arrow =  Instantiate(this.arrow, transform.position, Quaternion.identity);
            arrow.GetComponent<IarrowDamage>().damage = this.atk;
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

public interface IarrowDamage
{
    int damage { get; set; }
    GameObject target { get; set; }
}