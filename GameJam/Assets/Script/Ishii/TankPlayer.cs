using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseSystem.Property;

public class TankPlayer : BaseProp ,IPlayerProp
{
    private void Awake()
    {
        hp = 100;
        atk = 5;
    }

    private void Update()
    {
        
    }

    public void Attack()
    {

    }

    public void Dmage(int atk)
    {
        hp -= atk;
    }

    public void OnAtkChanged(int value)
    {
        throw new System.NotImplementedException();
    }

    public void OnHpChanged(int value)
    {
        throw new System.NotImplementedException();
    }
}
