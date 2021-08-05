using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int HP = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            HP -= (collision.GetComponent<Enemy>() as ISendDamage).deliverAtk;
            Destroy(collision.gameObject);
        }
    }
}

/// <summary>
/// EnemyÉNÉâÉXÇ…åpè≥Ç≥ÇπÇƒÇ≠ÇæÇ≥Ç¢ÅB
/// </summary>
public interface ISendDamage
{
    int deliverAtk { get; }
}
