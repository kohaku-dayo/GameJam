using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseSystem.Property;

public class CasterPlayer : BaseProp, IPlayerProp
{
    private bool m_AttackStart = false;
    public float m_time;
    [SerializeField] float m_intarval;
    public float Cost => 8;

    GameObject target;
    [SerializeField] GameObject magic;

    private void Awake()
    {
        SetCallback(this);
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
            Debug.Log("CastPlayer");
            // ������Enemy�Ƀ_���[�W��^���鏈��
            // Enemy�͖����̂ŁA�������u�ԃ_���[�W��^���鏈���͍s���܂���B
            // ��̃X�N���v�g�ɂă_���[�W�������s���Ă��������B
            // ���Archer��atk��J�ڂ���̂ŁAArcher�X�N���v�g�ɂ�IarrowDamage�C���^�[�t�F�[�X���������Ă��������B
            GameObject arrow = Instantiate(magic, transform.position, Quaternion.identity);
            arrow.GetComponent<IMagicDamage>().damage = this.atk;
            arrow.GetComponent<IMagicDamage>().target = target;
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
            target = other.gameObject;
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
    }

    public void OnHpChanged(int value)
    {
        Debug.Log("hi");
        if (value <= 0) Destroy(this.gameObject);
    }
}

public interface IMagicDamage
{
    int damage { get; set; }
    GameObject target { get; set; }
}