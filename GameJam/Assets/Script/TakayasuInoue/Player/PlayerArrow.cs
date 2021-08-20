using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using AppConst;
using UniRx;

/// <summary>
/// キャスターと特に振る舞いが変わらないのでキャスターにもアタッチ
/// </summary>
public class PlayerArrow : PlayerAbstract
{
    [SerializeField] GameObject m_bullet;
    [SerializeField] CharacterId m_characterId = default;

    public override void InitializePlayer(IManager manager)
    {
        base.InitializePlayer(manager);

        if (m_characterId == CharacterId.arrow)
        {
            m_audioManager.PlaySE(0);
        }
        else if (m_characterId == CharacterId.magic)
        {
            m_audioManager.PlaySE(3);
        }

        if (m_characterId == CharacterId.arrow)
        {
            m_hp
           .SkipLatestValueOnSubscribe()
           .Where(hp => hp <= 0)
           .Subscribe(_ => m_audioManager.PlaySE(2));
        }
        else if (m_characterId == CharacterId.magic)
        {
            m_hp
           .SkipLatestValueOnSubscribe()
           .Where(hp => hp <= 0)
           .Subscribe(_ => m_audioManager.PlaySE(5));
        }
    }

    protected override void AttackIntervalExcute()
    {
        if (m_target == null) return;
        m_anim.SetBool("Attack", true);
        if(m_characterId == CharacterId.arrow)
        {
            m_audioManager.PlaySE(1);
        }
        else if(m_characterId == CharacterId.magic)
        {
            m_audioManager.PlaySE(4);
        }
        InstantiateBullet(this.GetCancellationTokenOnDestroy()).Forget();
    }

    protected override void AttckUpdateExcute()
    {
        if(m_target == null)
        {
            SetTarget(m_manager.EnemyList);
        }
    }

    private async UniTask InstantiateBullet(CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.3f), false, PlayerLoopTiming.Update, cancellationToken);

        var bullet = Instantiate(m_bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<IBullet>().Initialize(this.gameObject, m_target);
        m_anim.SetBool("Attack", false);
    }
}



