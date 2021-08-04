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
    IObservable<Unit> Generate => this.UpdateAsObservable()
        .Where(_ => Input.GetMouseButtonDown(0));
    GameObject m_chara;

    bool m_isbuttonClick = true;

    // Start is called before the first frame update
    void Start()
    {
        m_battleView.EventSelect
            .Where(_=>m_isbuttonClick)
            .Subscribe(id => InstatiateCharacter(id));
        Generate
            .Where(_=> !m_isbuttonClick)
            .Subscribe(_ => RayInstantiate());
    }


    void RayInstantiate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject.tag != "Player")
            {
                Instantiate(m_chara, hit.point, Quaternion.identity);
            }
        }
        m_isbuttonClick = true;
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
