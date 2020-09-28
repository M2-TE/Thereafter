using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_Player;

    public void PlayAmbience(AudioClip clip, float vol)
    {
        m_Player.PlayOneShot(clip, vol);
    }
}
