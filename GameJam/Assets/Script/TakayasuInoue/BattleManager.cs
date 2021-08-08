using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

public interface IManager
{
    void AddCost(float cost);
    void ReduceCost(float cost);
    void AddEnemyList(GameObject gameObject);
    IObservable<Unit> StartGame { get; }
    IObservable<float> GameOver { get; }
    float SumCost { get; }
    List<GameObject> EnemyList { get; }
}
public class BattleManager : MonoBehaviour,IManager
{
    [SerializeField] private float sumCost = default;
    [SerializeField] private BattleView m_battleView = default;
    
    public float SumCost => sumCost;
    private float m_totalTime;
    public IObservable<Unit> StartGame => m_startGame;
    public IObservable<float> GameOver => m_gameOver;

    Subject<Unit> m_startGame = new Subject<Unit>();
    Subject<float> m_gameOver = new Subject<float>();
    public List<GameObject> EnemyList { get; private set; } = new List<GameObject>();

    private float m_maxCost;
    CancellationTokenSource m_cancellation = new CancellationTokenSource();

    private void Awake()
    {
        m_battleView.RefrectCost(sumCost,m_maxCost);
        m_maxCost = sumCost;
    }

    public void AddCost(float cost)
    {
        sumCost += cost;
        if (m_maxCost < sumCost) sumCost = m_maxCost;
     
   
        m_battleView.RefrectCost(sumCost , m_maxCost);
    }

    public void AddEnemyList(GameObject gameObject)
    {
        gameObject.GetComponent<IEnemyParameter>().Hp
            .Where(hp => hp <= 0)
            .Subscribe(_ => EnemyList.Remove(gameObject))
            .AddTo(this);

        EnemyList.Add(gameObject);
    }


    public void ReduceCost(float cost)
    {
        sumCost -= cost;
        if (sumCost < 0) sumCost = 0;

        m_battleView.RefrectCost(sumCost , m_maxCost);
    }

    void Start()
    {
        m_startGame.OnNext(Unit.Default);
        TimeCount(m_cancellation.Token).Forget();
    }

    async UniTask TimeCount(CancellationToken cancellation)
    {
        while (!cancellation.IsCancellationRequested)
        {
            m_totalTime += Time.deltaTime;
            var time = (float)Math.Floor((m_totalTime * 10) / 10);
            AddCost(0.6f * Time.deltaTime);
            m_battleView.RefrectTime(time);
            await UniTask.Yield(PlayerLoopTiming.Update, cancellation);
        }
    }

    public void GameOverExcute()
    {
        m_gameOver.OnNext(m_totalTime);
        m_cancellation.Cancel();
    }
}
