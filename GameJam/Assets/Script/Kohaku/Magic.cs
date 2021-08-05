using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Handy;

public class Magic : MonoBehaviour, IMagicDamage
{
    Rigidbody rb;

    public int damage { get; set; }
    public GameObject target { get; set; }
    [SerializeField] float speed;

    private void Awake()
    {
        Debug.Log("magic instanciated");
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {

        rb.velocity = target.transform.position - this.transform.position;
        //Tracer.trace(target, gameObject, rb, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.GetComponent<IEnemy>().Damage(damage);
            Destroy(this.gameObject);
        }
    }
}
