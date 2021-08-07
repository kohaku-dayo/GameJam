using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

public class GenerateEnemy : MonoBehaviour
{
    [SerializeField] GameObject m_enemy;
    [SerializeField] GameObject m_manager;
    [SerializeField] GameObject m_tower;
    private List<GameObject> m_enemyList = new List<GameObject>();
    CancellationTokenSource m_cancellationToken = new CancellationTokenSource();

    float count = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_manager.GetComponent<IManager>().GameOver.Subscribe(_=> m_cancellationToken.Cancel());
        InstantiateEnemyAsync(m_cancellationToken.Token).Forget();
    }


    async UniTask InstantiateEnemyAsync(CancellationToken cancellation)
    {
        var time = (float)UnityEngine.Random.Range(1, 30);
        while(true)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                var enemy = Instantiate(m_enemy, this.transform.position, Quaternion.AngleAxis(90, Vector3.right));

                enemy.GetComponent<Enemy>().Initialize(m_tower, m_manager.GetComponent<IManager>());
                count++;
                enemy.gameObject.name = count.ToString() + transform.position;
                time = UnityEngine.Random.Range(1, 30);
            }
            await UniTask.Yield(PlayerLoopTiming.Update,cancellation);
        }
    }
}
