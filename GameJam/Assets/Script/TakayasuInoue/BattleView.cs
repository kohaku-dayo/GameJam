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
    [SerializeField] ButtonController m_slectButton1 = default;
    [SerializeField] ButtonController m_slectButton2 = default;
    [SerializeField] ButtonController m_slectButton3 = default;
    [SerializeField] ButtonController m_slectButton4 = default;

    public IObservable<CharacterId> EventSelect => Observable.Merge(
        m_slectButton1.ButtonClick,
        m_slectButton2.ButtonClick,
        m_slectButton3.ButtonClick
        ,m_slectButton4.ButtonClick);
  




    private void Awake()
    {
      

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
