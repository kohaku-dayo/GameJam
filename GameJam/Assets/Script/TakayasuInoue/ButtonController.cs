using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;
using AppConst;

public class ButtonController : MonoBehaviour
{
    [SerializeField] Button m_selectBuuton = default;
    [SerializeField] Image m_buttonImage = default;
    [SerializeField] AudioSource m_clickSound = default;
    [SerializeField] CharacterId m_characterId;
    [SerializeField] Outline m_outine;

    public IObservable<CharacterId> ButtonClick => m_selectBuuton
        .OnClickAsObservable()
        .Select(_ => m_characterId);


    private void Awake()
    {
        //ButtonClick.Subscribe(_ => m_clickSound.Play());
        ButtonClick
            .SelectMany(Observable.Timer(TimeSpan.FromSeconds(0.1f)))
            .Subscribe(_ => SelectImage());
    }

    public void SelectImage()
    {
        //m_buttonImage.color = Color.red;
        m_outine.enabled = true;
    }

    public void SelectCancelImage()
    {
        //m_buttonImage.color = Color.white;
        m_outine.enabled = false;
    }

    /// <summary>
    /// 外部からimageをセットする用
    /// </summary>
    /// <param name="image"></param>
    public void SetImage(Image image)
    {
        m_buttonImage = image;
    }
}
