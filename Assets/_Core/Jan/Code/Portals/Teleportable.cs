using System;
using UnityEngine;

[DefaultExecutionOrder(-30004)]
public class Teleportable : MonoBehaviour
{
    protected GameObject m_copy;
    [NonSerialized] public Portal m_cachedPortal;
    private void OnEnable()
    {
        Application.onBeforeRender += PortalProcedure;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= PortalProcedure;
    }

    protected virtual void Awake()
    {
        m_copy = new GameObject();
        m_copy.layer = gameObject.layer;
        m_copy.name = name + "_portalCopy";
        m_copy.AddComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
        m_copy.AddComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
        m_copy.SetActive(false);
    }


    protected virtual void PortalProcedure()
    {
        if (m_cachedPortal == null) return;
        m_cachedPortal.MirrorPosition(transform, m_copy.transform);
        m_cachedPortal.MirrorRotation(transform, m_copy.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        m_cachedPortal = other.GetComponent<Portal>();
        m_copy.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        m_cachedPortal = null;
        m_copy.SetActive(false);
    }
}
