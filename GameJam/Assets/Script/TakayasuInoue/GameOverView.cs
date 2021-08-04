using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
    [SerializeField] GameObject m_battleManager = default;
    [SerializeField] GameObject m_resultPanel = default;
    [SerializeField] Text m_totalTimeText = default;
    IManager manager;

    // Start is called before the first frame update
    void Start()
    {
        m_battleManager.GetComponent<IManager>().GameOver
            .Subscribe(time =>
            {
                m_totalTimeText.text = $"{time.ToString()}ê∂Ç´écÇ¡ÇΩ";
                m_resultPanel.SetActive(true);
            });
    }
}
