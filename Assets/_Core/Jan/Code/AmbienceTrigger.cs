using UnityEngine;

public class AmbienceTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;

    private void OnTriggerEnter(Collider other)
    {
        m_AudioSource.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        m_AudioSource.Stop();
    }
}
