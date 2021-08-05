using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;

public interface IManager
{
    void AddCost(float cost);
    void ReduceCost(float cost);
    IObservable<Unit> StartGame { get; }
    IObservable<float> GameOver { get; }

    float SumCost { get; }
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

    bool isCountTime = true;

    private void Awake()
    {
        m_battleView.RefrectCost(sumCost);

    }

    private void Update()
    {
        //sumCost += Time.deltaTime;
    }
    public void AddCost(float cost)
    {
     
        sumCost += cost;
        Debug.Log(sumCost);
        m_battleView.RefrectCost(sumCost);
    }

    public void ReduceCost(float cost)
    {
        sumCost -= cost;
        Debug.Log(sumCost);
        m_battleView.RefrectCost(sumCost);
    }

    void Start()
    {
        m_startGame.OnNext(Unit.Default);
        TimeCount().Forget();
    }

    async UniTask TimeCount()
    {
        while (isCountTime)
        {
            m_totalTime += Time.deltaTime;
            var time = (float)Math.Floor((m_totalTime * 10) / 10);

            m_battleView.RefrectTime(time);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    public void GameOverExcute()
    {
        m_gameOver.OnNext(m_totalTime);
    }
}
