using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class csDestroyEffect : MonoBehaviour {

    private void Awake()
    {
        DestroyEffect().Forget();
    }

    async UniTask DestroyEffect()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        Destroy(this.gameObject);
    }
}
