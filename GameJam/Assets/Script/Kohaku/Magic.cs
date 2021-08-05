using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Handy;

public class Magic : MonoBehaviour, IMagicDamage
{
    Rigidbody rb;

    public int damage { get; set; }
    public GameObject target { get; set; }
    GameObject m_target;
    [SerializeField] float m_speed;
    [SerializeField] int m_Damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        m_target = GameObject.FindWithTag("Enemy");
    }
    public void SetTarget(GameObject target)
    {
        Debug.Log("Set");
        m_target = target;
    }
    private void FixedUpdate()
    {
        Debug.Log("Bullet");

        rb.velocity = (m_target.transform.position - this.transform.position).normalized * m_speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.GetComponent<IEnemy>().Damage(m_Damage);
            Destroy(this.gameObject);
        }
    }
}
