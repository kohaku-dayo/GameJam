using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;

public class GenerateEnemy : MonoBehaviour
{
    [SerializeField] GameObject m_enemy;
    [SerializeField] GameObject m_manager;
    GameObject m_tower;
    bool isFinish = false;
    // Start is called before the first frame update
    void Start()
    {
        m_tower = GameObject.FindGameObjectWithTag("Tower");
        m_manager.GetComponent<IManager>().GameOver.Subscribe(_=> isFinish = true);
        LoopInstantiateEnemyAsync().Forget();
    }

    async UniTask LoopInstantiateEnemyAsync()
    {
        while (!isFinish)
        {
            await InstantiateEnemyAsync();
        }
    }

    async UniTask InstantiateEnemyAsync()
    {
        float time = UnityEngine.Random.Range(1, 30);

        while (time > 0)
        {
            time -= Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        var enemy = Instantiate(m_enemy, this.transform.position, Quaternion.AngleAxis(90, Vector3.right));

        enemy.GetComponent<PlayerChaser>().SetTowerGameObject(m_tower);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
