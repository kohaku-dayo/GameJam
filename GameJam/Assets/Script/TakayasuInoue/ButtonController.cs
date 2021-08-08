using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;
using AppConst;
using DG.Tweening;

public class ButtonController : MonoBehaviour
{
    [SerializeField] Button m_selectBuuton = default;
    [SerializeField] Image m_buttonImage = default;
    [SerializeField] AudioSource m_clickSound = default;
    [SerializeField] CharacterId m_characterId;
    [SerializeField] Outline m_outine;
    [SerializeField] Text m_costText = default;

    public IObservable<CharacterId> ButtonClick => m_selectBuuton
        .OnClickAsObservable()
        .Select(_ => m_characterId);


    private void Awake()
    {
        ButtonClick
            .SelectMany(Observable.Timer(TimeSpan.FromSeconds(0.01f)))
            .Subscribe(_ => SelectImage())
            .AddTo(this);

        ButtonClick.Subscribe(_ =>
        {
            transform.DOPunchScale(
                    punch: Vector3.one * 0.1f,
                    duration: 0.2f,
                    vibrato: 1
                ).SetEase(Ease.OutExpo);
        })
        .AddTo(this);
    }

    public void SelectImage()
    {
        m_outine.enabled = true;
    }

    public void SelectCancelImage()
    {
        m_outine.enabled = false;
    }

    public void SetButtonCostText(float cost)
    {
        m_costText.text = $" コスト{cost.ToString()}";
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
