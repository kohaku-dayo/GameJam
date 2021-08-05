using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseSystem.Property;

public class CasterPlayer : BaseProp, IPlayerProp
{
    private bool m_AttackStart = false;
    private float m_time;
    [SerializeField] float m_intarval;
    public float Cost => _cost;
    public bool IsDead{ get; private set; }
    GameObject target;
    [SerializeField] GameObject magic;
    [SerializeField] int _hp;
    [SerializeField] int _attck;
    [SerializeField] float _cost;
    [SerializeField] AudioSource m_audio;
    [SerializeField]Animator m_anim;
    
    private void Awake()
    {
        SetCallback(this);
        hp = _hp;
        atk = _attck;
    }


    private void Update()
    {
       
            m_time += Time.deltaTime;
        //m_anim.SetBool("Attack",false);
        m_anim.SetBool("Attack", true);


        if (m_time > m_intarval)
        {
            Debug.Log("CastPlayer");
            // ここにEnemyにダメージを与える処理
            // Enemyは矢を放つので、放った瞬間ダメージを与える処理は行いません。
            // 矢のスクリプトにてダメージ処理を行ってください。
            // 矢へArcherのatkを遷移するので、ArcherスクリプトにてIarrowDamageインターフェースを実装してください。
            //GameObject arrow = Instantiate(magic, transform.position, Quaternion.identity);

            //arrow.GetComponent<Magic>().SetTarget(target);

            //arrow.GetComponent<IMagicDamage>().damage = this.atk;
            //arrow.GetComponent<IMagicDamage>().target = target;
    
            target.GetComponent<IEnemy>().Damage(atk);
            m_time = 0;

            //m_audio.Play();
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
        if (value <= 0)
        {
            IsDead = true;
            Destroy(this.gameObject);
        }
    }
}

public interface IMagicDamage
{
    int damage { get; set; }
    GameObject target { get; set; }
}