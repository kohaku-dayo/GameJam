using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using AppConst;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

public class GenerateCharacter : MonoBehaviour
{
    [SerializeField] List<GameObject> m_character;
    [SerializeField] BattleView m_battleView = default;
    [SerializeField] GameObject m_battleManager = default;
    [SerializeField] GameObject m_effect = default;

    private IManager m_IManager;
    private IObservable<Unit> Generate => this.UpdateAsObservable()
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


    private void RayInstantiate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject.tag == "Terrain")
            {
                var cost = m_chara.GetComponent<IPlayerParameter>().Cost.Value;
                var sumCost = m_IManager.SumCost;
                var upadateCost = sumCost - cost;
                if (upadateCost <= 0) return;

                InstantiateChara(hit.point).Forget();
                m_IManager.ReduceCost(cost);
            }            
        }
    }

    private async UniTask<GameObject> InstantiateChara(Vector3 pos)
    {
        pos.y = 0;
        Instantiate(m_effect, pos, Quaternion.identity);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        var chara = Instantiate(m_chara, pos, Quaternion.identity);
        chara.GetComponent<PlayerAbstract>().InitializePlayer(m_IManager);

        return chara;
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
