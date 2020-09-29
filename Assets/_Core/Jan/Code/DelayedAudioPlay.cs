using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAudioPlay : MonoBehaviour
{
    [SerializeField] private AudioSource m_Audio;
    [SerializeField] private float m_Delay;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(m_Delay);
        m_Audio.Play();
    }
}
