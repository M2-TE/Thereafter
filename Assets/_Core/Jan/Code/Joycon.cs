using UnityEngine;

public class Joycon : MonoBehaviour
{
    [SerializeField] private bool m_isLeftHand;
    [SerializeField] private float m_sphereScanRadius;
    [SerializeField] private LayerMask m_groundLayer;
    [SerializeField] private LayerMask m_obstacleLayer;

    private XRInput m_input;
    private GameObject m_sphere;
    private Material m_sphereMat;

    private void Start()
    {
        m_input = XRInput.Instance;

        m_sphere = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Destroy(m_sphere.GetComponent<Collider>());
        m_sphere.transform.localScale = new Vector3(m_sphereScanRadius, .5f, m_sphereScanRadius) * 2f;
        m_sphereMat = m_sphere.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        // TODO move to fixed update
        if(m_input.GetTrigger(m_isLeftHand))
        {
            if (Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hit, 10f, m_groundLayer))
            {
                m_sphere.SetActive(true);
                m_sphere.transform.position = hit.point;

                if (Physics.CheckSphere(hit.point, m_sphereScanRadius, m_obstacleLayer))
                {
                    m_sphereMat.color = Color.red;
                }
                else
                {
                    m_sphereMat.color = Color.white;
                }
            }
            else
            {
                m_sphere.SetActive(false);
            }
        }
    }
}
