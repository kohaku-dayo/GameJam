using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    void Initialize(GameObject owner, GameObject target);
    void SetAttackParameter(float attack);
    void SetSpeed(float speed);
}
