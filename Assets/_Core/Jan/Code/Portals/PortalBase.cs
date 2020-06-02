using UnityEngine;

public class PortalBase : MonoBehaviour
{
    [SerializeField] protected PortalBase m_Pair;

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
