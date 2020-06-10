using System;
using UnityEngine;

[DefaultExecutionOrder(-30004)]
public class Teleportable : MonoBehaviour
{
    [SerializeField] private Mesh mesh;
    [NonSerialized] public Portal m_cachedPortal;
    protected GameObject m_copy;
    private void OnEnable()
    {
        Application.onBeforeRender += PortalProcedure;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= PortalProcedure;
    }

    protected void Start()
    {
        m_copy = new GameObject();
        m_copy.layer = gameObject.layer;
        m_copy.name = name + "_portalCopy";
        if (GetComponent<SkinnedMeshRenderer>() == null)
        {
            m_copy.AddComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
            m_copy.AddComponent<MeshRenderer>().material = GetComponent<Renderer>().material;
        }
        else
        {
            //Mesh mesh = new Mesh();
            GetComponent<SkinnedMeshRenderer>().BakeMesh(mesh);
            m_copy.AddComponent<MeshFilter>().mesh = mesh;
            m_copy.AddComponent<MeshRenderer>().material = GetComponent<SkinnedMeshRenderer>().material;
        }
        m_copy.SetActive(false);
    }

    protected void PortalProcedure()
    {
        if (m_cachedPortal == null) return;
        m_cachedPortal.Mirror(transform, m_copy.transform);
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
