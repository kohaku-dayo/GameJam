using System;
using UniRx;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseProp
{
    public void OnHpChanged(int value);
    void OnAtkChanged(int value);
}

public class BaseProp
{
    protected List<IBaseProp> targets = new List<IBaseProp>();

    int _hp;
    int _atk;

    protected int hp
    {
        get => _hp;
        set
        {
            _hp = value;
            callHpCallback(value);
        }
    }
    protected int atk
    {
        get => _atk;
        set
        {
            _atk = value;
            callAtkCallback(value);
        }
    }

    /// <summary>
    /// コールバック対象を自分にすることでインターフェースに定義されている関数を呼んでくれます！
    /// </summary>
    /// <param name="target"></param>
    public void SetCallback(IBaseProp target)
    {
        targets.Add(target);
    }

    void callHpCallback(int value)
    {
        foreach (var target in targets) target.OnHpChanged(value);
    }

    void callAtkCallback(int value)
    {
        foreach (var target in targets) target.OnAtkChanged(value);
    }


}