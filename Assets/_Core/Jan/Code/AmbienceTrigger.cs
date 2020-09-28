using UnityEngine;

public class AmbienceTrigger : MonoBehaviour
{
    [SerializeField] private AudioManager m_Manager;
    [SerializeField] private AudioClip m_EntryRoomAmbience;
    [SerializeField] private float m_Vol = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        m_Manager.PlayAmbience(m_EntryRoomAmbience, m_Vol);
    }
}
