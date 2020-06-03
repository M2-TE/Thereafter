using UnityEngine;

public class PortalBase : MonoBehaviour
{
    [SerializeField] private GameObject m_QuadDisplayRight;
    [SerializeField] private GameObject m_QuadDisplayLeft;
    private GameObject m_QuadCopyRight;
    private GameObject m_QuadCopyLeft;

    public PortalBase m_Pair;

    private void Awake()
    {
        m_QuadCopyLeft = Instantiate(m_QuadDisplayLeft, transform);
        m_QuadCopyRight = Instantiate(m_QuadDisplayRight, transform);
    }

    private void Update()
    {
        Vector3 vec = transform.forward * PlayerController.s_EyeSeperation;
        m_QuadCopyLeft.transform.localPosition = vec;
        m_QuadCopyRight.transform.localPosition = vec;
    }

    public void MirrorPosition(Transform original, Transform target)
    {
        var relativePosition = transform.InverseTransformPoint(original.position);
        relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
        target.position = m_Pair.transform.TransformPoint(relativePosition);
    }

    public void MirrorRotation(Transform original, Transform target)
    {
        var relativeRotation = transform.InverseTransformDirection(original.forward);
        relativeRotation = Vector3.Scale(relativeRotation, new Vector3(-1, 1, -1));
        target.forward = m_Pair.transform.TransformDirection(relativeRotation);

        // z adjustment
        target.Rotate(new Vector3(0f, 0f, original.rotation.eulerAngles.z));
    }

}
