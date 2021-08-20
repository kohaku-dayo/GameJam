using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameOverView : MonoBehaviour
{
    [SerializeField] GameObject m_battleManager = default;
    [SerializeField] GameObject m_resultPanel = default;
    [SerializeField] Text m_totalTimeText = default;
    [SerializeField] Button m_titleButton = default;
    IManager manager;

    // Start is called before the first frame update
    void Start()
    {
        m_battleManager.GetComponent<IManager>().GameOver
            .Subscribe(time =>
            {
                var times = (float)Math.Floor((time * 10) / 10);
                m_totalTimeText.text = $"{times.ToString()}•b¶‚«Žc‚Á‚½";
                m_resultPanel.SetActive(true);
            });

        m_titleButton.onClick.AddListener(() => SceneManager.LoadScene("Start"));
    }
}
