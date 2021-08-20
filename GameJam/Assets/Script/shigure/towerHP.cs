using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;

public class towerHP : MonoBehaviour,IDamage
{
    int maxHp = 50;
    float crenntHP;
    
    int Zakoenemy = 5;
    int Mediumenemy = 5;
    int Greatenemy = 10;

    [SerializeField] GameObject m_effect = default;

    private AudioManager m_audioManager;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = 50;
        crenntHP = maxHp;
        m_audioManager = FindObjectOfType<AudioManager>();
    }
    bool delay = false;
    public void Damage(float attack)
    {
        crenntHP -= attack;
        slider.value = crenntHP;

        if (!delay)
        {
            DelaySound().Forget();
            delay = true;
        }

        Effect().Forget();

        if (crenntHP <= 0)
        {
            GameObject.Find("BattleManager").GetComponent<BattleManager>().GameOverExcute();
            Destroy(this.gameObject);
        }
    }

    async UniTask DelaySound()
    {
        m_audioManager.PlaySE(12);
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        delay = false;
    }

    async UniTask Effect()
    {
        var effects = Instantiate(m_effect, transform.position, Quaternion.AngleAxis(90, Vector3.right));
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        Destroy(effects);

    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log( crenntHP);

    }
}
