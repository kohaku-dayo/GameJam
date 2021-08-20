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
    [SerializeField] GameObject m_blueEnemy;
    [SerializeField] GameObject m_redEnemy;
    [SerializeField] GameObject m_manager;
    [SerializeField] GameObject m_tower;
    AudioManager m_audiomanager;
    CancellationTokenSource m_cancellationToken = new CancellationTokenSource();

    private IManager Imanager;
    // Start is called before the first frame update
    void Start()
    {
        Imanager = m_manager.GetComponent<IManager>();
        m_audiomanager = FindObjectOfType<AudioManager>();
        Imanager.GameOver.Subscribe(_=> m_cancellationToken.Cancel());
        InstantiateEnemyAsync(m_cancellationToken.Token).Forget();
    }


    async UniTask InstantiateEnemyAsync(CancellationToken cancellation)
    {
        var time = (float)UnityEngine.Random.Range(1, 30);
        var redTime = (float)UnityEngine.Random.Range(1, 100);
        var blueTime = (float)UnityEngine.Random.Range(1, 200);
   
        while(true)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                var enemy = Instantiate(m_enemy, this.transform.position, Quaternion.identity);

                enemy.GetComponent<Enemy>().Initialize(m_tower, Imanager);

                Imanager.AddEnemyList(enemy);

                time = UnityEngine.Random.Range(1, 30);
            }

            redTime -= Time.deltaTime;
            if (redTime <= 0)
            {
                var enemy = Instantiate(m_enemy, this.transform.position, Quaternion.identity);

                enemy.GetComponent<Enemy>().Initialize(m_tower, Imanager);

                Imanager.AddEnemyList(enemy);

                m_audiomanager.PlaySE(10);

                redTime = UnityEngine.Random.Range(1, 100);
            }

            blueTime -= Time.deltaTime;
            if (blueTime <= 0)
            {
                var enemy = Instantiate(m_enemy, this.transform.position, Quaternion.identity);

                enemy.GetComponent<Enemy>().Initialize(m_tower, Imanager);

                Imanager.AddEnemyList(enemy);

                m_audiomanager.PlaySE(11);

                blueTime = UnityEngine.Random.Range(1, 200);
            }


            await UniTask.Yield(PlayerLoopTiming.Update,cancellation);
        }
    }
}
