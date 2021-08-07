using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using AppConst;
using UnityEngine.EventSystems;

public class GenerateCharacter : MonoBehaviour
{
    [SerializeField] List<GameObject> m_character;
    [SerializeField] BattleView m_battleView = default;
    [SerializeField] GameObject m_battleManager = default;
    IManager m_IManager;
    IObservable<Unit> Generate => this.UpdateAsObservable()
        .Where(_ => Input.GetMouseButtonDown(0));
    private GameObject m_chara;

    // Start is called before the first frame update
    void Start()
    {     
        m_battleView.EventSelect
            .Subscribe(id => InstatiateCharacter(id));

        Generate
            .Where(_=> EventSystem.current.currentSelectedGameObject == null)
            .Subscribe(_ => RayInstantiate());

        m_IManager = m_battleManager.GetComponent<IManager>();

        CostRefrect();
    }


    void RayInstantiate()
    {
        Debug.Log("Ray");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject.tag != "PlayerCollider" && hit.collider.gameObject.tag != "Enemy")
            {
                if (m_IManager.SumCost <= 0) return;

                Debug.Log(hit.collider.gameObject.tag);
                var pos = hit.point;
                pos.y = 0;
                var chara = Instantiate(m_chara, pos,Quaternion.identity);

                var cost = chara.GetComponent<IPlayerParameter>().Cost.Value;
                m_IManager.ReduceCost(cost);
            }            
        }
    }

    private void CostRefrect()
    {
        List<float> cost = new List<float>();
        foreach(var c in m_character)
        {
            cost.Add(c.GetComponent<IPlayerParameter>().Cost.Value);
        }
        m_battleView.RefrectButtonCost(cost);
    }


    void InstatiateCharacter(CharacterId id)
    {
        switch (id)
        {
            case CharacterId.sord:
                m_chara = m_character[0];

                break;
            case CharacterId.arrow:
                m_chara = m_character[1];

                break;
            case CharacterId.magic:
                m_chara = m_character[2];
   
                break;
            case CharacterId.kabe:
                m_chara = m_character[3];

                break;
        }
   
    }

    /// <summary>
    /// コードでprefabの参照を渡す用
    /// </summary>
    /// <param name="character"></param>
    public void SetCharacterInstance(List<GameObject> character)
    {
        m_character = character;
    }
}
