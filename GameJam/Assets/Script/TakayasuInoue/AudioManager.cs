using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource m_audioSource = default;
    [SerializeField] List<AudioClip> m_seList = default;

    public  void PlaySE(int num)
    {
        m_audioSource.clip = m_seList[num];

        m_audioSource.PlayOneShot(m_seList[num]);
    }

}
