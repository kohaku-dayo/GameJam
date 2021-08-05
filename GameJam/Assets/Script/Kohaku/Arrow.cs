using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Handy;

public class Arrow : MonoBehaviour, IarrowDamage
{
    Rigidbody rb;

    public int damage { get; set; }
    public GameObject target { get; set; }

    private void Awake()
    {
        Debug.Log("arrow instanceated");
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Tracer.trace(target, gameObject, rb);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<IEnemy>().Damage(damage);
            Destroy(this.gameObject);
        }
    }
}
