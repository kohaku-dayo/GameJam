using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using AppConst;

public class GenerateCharacter : MonoBehaviour
{
    [SerializeField] List<GameObject> m_character;
    [SerializeField] BattleView m_battleView = default;
    [SerializeField] GameObject m_battleManager = default;
    IManager m_IManager;
    IObservable<Unit> Generate => this.UpdateAsObservable()
        .Where(_ => Input.GetMouseButtonDown(0));
    GameObject m_chara;

    bool m_isbuttonClick = true;
    float m_sumCost = 20;
    float m_cost;

    // Start is called before the first frame update
    void Start()
    {
        m_battleView.EventSelect
            .Where(_=>m_isbuttonClick)
            .Subscribe(id => InstatiateCharacter(id));
        Generate
            .Where(_=> !m_isbuttonClick)
            .Subscribe(_ => RayInstantiate());
        m_battleView.RefrectCost(m_sumCost);

        m_IManager = m_battleManager.GetComponent<IManager>();
    }


    void RayInstantiate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            if (
               m_IManager.SumCost < m_chara.GetComponent<IPlayerProp>().Cost)
            {
                m_battleView.SelectCancel();
                m_isbuttonClick = true;
                return;
            }
            var chara = Instantiate(m_chara, hit.point, Quaternion.AngleAxis(90, Vector3.right));
            var cost = chara.GetComponent<CasterPlayer>().Cost;
            //m_IManager.ReduceCost(cost);
            GameObject.Find("BattleManager").GetComponent<BattleManager>().ReduceCost(cost);
            
        }
        m_battleView.SelectCancel();
        m_isbuttonClick = true;
    }

    void InstatiateCharacter(CharacterId id)
    {
        switch (id)
        {
            case CharacterId.sord:
                m_chara = m_character[0];
                m_cost = 5;
                break;
            case CharacterId.arrow:
                m_chara = m_character[1];
                m_cost = 5;

                break;
            case CharacterId.magic:
                m_chara = m_character[2];
                m_cost = 5;

                break;
            case CharacterId.kabe:
                m_chara = m_character[3];
                m_cost = 5;

                break;
        }
        m_isbuttonClick = false;
    }

    /// <summary>
    /// コードでprefabの参照を渡す
    /// </summary>
    /// <param name="character"></param>
    public void SetCharacterInstance(List<GameObject> character)
    {
        m_character = character;
    }
}
