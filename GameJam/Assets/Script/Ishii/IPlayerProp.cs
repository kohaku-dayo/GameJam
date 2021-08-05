using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerProp : IBaseProp
{
    void Dmage(int damage);
    bool IsDead { get; }
    float Cost { get; } 
}
