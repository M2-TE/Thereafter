using UnityEngine;

[DefaultExecutionOrder(-30000)]
public class Teleportable : MonoBehaviour
{
    private GameObject m_copy;
    private PortalBase m_cachedPortal;
    private void OnEnable()
    {
        Application.onBeforeRender += UpdateCopy;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= UpdateCopy;
    }

    private void Awake()
    {
        m_copy = new GameObject();
        m_copy.layer = gameObject.layer;
        m_copy.name = name + "_portalCopy";
        m_copy.AddComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
        m_copy.AddComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
        m_copy.SetActive(false);
    }

    private void Update()
    {
        //UpdateCopy();
    }

    private void UpdateCopy()
    {
        Debug.Log("teleporting " + Time.time);
        if (m_cachedPortal == null) return;
        m_cachedPortal.MirrorPosition(transform, m_copy.transform);
        m_cachedPortal.MirrorRotation(transform, m_copy.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        m_cachedPortal = other.GetComponent<PortalBase>();
        m_copy.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        m_cachedPortal = null;
        m_copy.SetActive(false);
    }
}
