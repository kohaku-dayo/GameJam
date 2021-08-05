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
    public bool IsDead { get; private set; }
    private GameObject enemy;
    IEnemy target_IEnemy;
    [SerializeField] GameObject arrow;
    [SerializeField] int _hp;
    [SerializeField] int _attck;
    private void Awake()
    {
        hp = _hp;
        atk = _attck;
    }

    private void Update()
    {
        if (m_AttackStart == true)
        {
            m_time += Time.deltaTime;
        }
        if (m_time > m_intarval)
        {
            // ������Enemy�Ƀ_���[�W��^���鏈��
            // Enemy�͖����̂ŁA�������u�ԃ_���[�W��^���鏈���͍s���܂���B
            // ��̃X�N���v�g�ɂă_���[�W�������s���Ă��������B
            // ���Archer��atk��J�ڂ���̂ŁAArcher�X�N���v�g�ɂ�IarrowDamage�C���^�[�t�F�[�X���������Ă��������B
            //GameObject arrow =  Instantiate(this.arrow, transform.position, Quaternion.identity);
            //arrow.GetComponent<Magic>().SetTarget(enemy);
            
            //arrow.GetComponent<IarrowDamage>().damage = this.atk;
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
            enemy = other.gameObject;
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
        if (value <= 0)
        {
            IsDead = true;
            Destroy(this.gameObject);
        }
    }
}

public interface IarrowDamage
{
    int damage { get; set; }
    GameObject target { get; set; }
}