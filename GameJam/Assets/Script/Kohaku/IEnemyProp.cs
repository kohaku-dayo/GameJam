using System;
using UniRx;
using System.Collections.Generic;
using UnityEngine;
using BaseSystem.Property;

public interface IEnemy : IBaseProp
{
    void Damage(int attack);
    void SetTarget(GameObject taget);
    public int Attack { get; set; }
}

namespace BaseSystem.Property
{
    public class EnemyProp : BaseProp{
        protected int searchRange;
    }

}