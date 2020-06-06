using UnityEngine;
using UnityEngine.SpatialTracking;

[DefaultExecutionOrder(-30005)]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool m_IsMouseControlled;
    [Header("Trackers and Cams")]
    [SerializeField] private Camera m_CamHMD;
    [SerializeField] private Transform m_EyeRight;
    [SerializeField] private Transform m_EyeLeft;
    [SerializeField] private Teleportable[] teleportables;

    [Header("Movement")]
    [SerializeField] private float m_Movespeed = .05f;
    [SerializeField] private float m_MouseLookSpeed = 1f;

    public static float s_EyeSeperation = .05f;
    private Vector3 m_CurrentRotation = Vector3.zero;
    private Portal m_CurrentPortal;

    private void OnEnable()
    {
        Application.onBeforeRender += UpdatePortalProcedures;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= UpdatePortalProcedures;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        s_EyeSeperation = Vector3.Distance(m_EyeRight.position, m_EyeLeft.position);

        if(Input.GetKeyDown(KeyCode.T))
        {
            m_IsMouseControlled = !m_IsMouseControlled;
        }

        if(!m_IsMouseControlled)
        {
            m_CamHMD.GetComponent<TrackedPoseDriver>().enabled = true;
        }
        else
        {
            m_CamHMD.GetComponent<TrackedPoseDriver>().enabled = false;

            float vMouse = Input.GetAxis("Mouse X") * m_MouseLookSpeed;
            float hMouse = Input.GetAxis("Mouse Y") * -m_MouseLookSpeed;

            m_CurrentRotation = new Vector3(
                Mathf.Clamp(m_CurrentRotation.x + hMouse, -90f, 90f),
                m_CurrentRotation.y + vMouse,
                0f);

            m_CamHMD.transform.rotation = Quaternion.Euler(m_CurrentRotation);
        }

        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");
        var moveVec = m_CamHMD.transform.forward * vInput + m_CamHMD.transform.right * hInput;
        moveVec.y = 0f;
        transform.Translate(moveVec.normalized * m_Movespeed, Space.World);
    }

    private void UpdatePortalProcedures()
    {
        if (m_CurrentPortal == null) return;

        var relVec = m_CamHMD.transform.position - m_CurrentPortal.transform.position;
        var angle = Vector3.Angle(m_CurrentPortal.transform.forward, relVec);
        if (angle < 90f)
        {
            m_CurrentPortal.MirrorPosition(transform, transform);
            m_CurrentPortal.MirrorRotation(transform, transform);

            foreach(var teleportable in teleportables)
            {
                teleportable.m_cachedPortal = m_CurrentPortal.m_Pair;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        m_CurrentPortal = other.GetComponent<Portal>();
    }

    private void OnTriggerExit(Collider other)
    {
        m_CurrentPortal = null;
    }
}
