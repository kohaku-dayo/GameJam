using System;
using UniRx;
using System.Collections.Generic;
using UnityEngine;
using BaseSystem.Property;

public interface IEnemy : IBaseProp{}

namespace BaseSystem.Property
{
    public class EnemyProp : BaseProp{
        protected int searchRange;
    }

}