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
    [SerializeField] Slider m_costSlidee = default;
    [SerializeField] Text m_time = default;
    [SerializeField] Text m_costText = default;

    public IObservable<CharacterId> EventSelect => m_selectButton
        .ToObservable()
        .SelectMany(b => b.ButtonClick);

    private void Awake()
    {
        SelectCancel();
        EventSelect.Subscribe(_ => SelectCancel());
    }

    public void RefrectCost(float cost,float maxCost)
    {
        //m_cost.text = $"ÉRÉXÉg{cost.ToString()}";
        m_costText.text = Math.Floor((cost * 10) / 10).ToString();
        m_costSlidee.value = cost / maxCost;
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
   

    public void RefrectButtonCost(List<float> cost)
    {
        int i = 0;
        foreach(var b in m_selectButton)
        {
            b.SetButtonCostText(cost[i]);
            i++;
        }
    }
 
}
