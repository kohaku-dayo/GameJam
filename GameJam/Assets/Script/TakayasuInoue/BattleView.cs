using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using System;
using AppConst;

public class BattleView : MonoBehaviour
{
    [SerializeField] List<ButtonController> m_selectButton = new List<ButtonController>();
    [SerializeField] Text m_cost = default;
    [SerializeField] Text m_time = default;

    public IObservable<CharacterId> EventSelect => Observable.Merge(
        m_selectButton[0].ButtonClick,
        m_selectButton[1].ButtonClick,
        m_selectButton[2].ButtonClick
        ,m_selectButton[3].ButtonClick);

    private void Awake()
    {
        SelectCancel();
        EventSelect.Subscribe(_ => SelectCancel());
    }

    public void RefrectCost(float cost)
    {
        //m_cost.text = $"ÉRÉXÉg{cost.ToString()}";
        m_cost.text = cost.ToString();

    }

    public void RefrectTime(float time)
    {
        m_time.text = time.ToString();
    }

    public void SelectCancel()
    {
        foreach(var b in m_selectButton)
        {
            b.SelectCancelImage();
        }
    }
   
 
}
