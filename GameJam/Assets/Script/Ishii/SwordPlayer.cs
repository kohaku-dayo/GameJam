using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseSystem.Property;

public class SwordPlayer : BaseProp, IPlayerProp
{
    public float Cost => 8;

    public void Dmage(int damage)
    {
        throw new System.NotImplementedException();
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
