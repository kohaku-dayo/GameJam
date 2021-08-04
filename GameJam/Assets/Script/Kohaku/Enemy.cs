using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseSystem.Property;

public class Enemy : EnemyProp, IEnemy
{
    [Header("Parameter")]
    [SerializeField] int HP = 10;
    [SerializeField] int ATK = 2;

    private void Awake()
    {
        hp = HP;
        atk = ATK;
    }

    public void OnAtkChanged(int value)
    {
        throw new System.NotImplementedException();
    }

    public void OnHpChanged(int value)
    {
        if (hp <= 0) Destroy(this);
    }
}
