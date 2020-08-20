using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SpatialTracking;

[DefaultExecutionOrder(-30005)]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool m_IsMouseControlled;
    [SerializeField] private bool m_IsDirectionallyControlled;
    [Header("Trackers and Cams")]
    [SerializeField] private Camera m_CamHMD;
    [SerializeField] private Transform m_EyeRight;
    [SerializeField] private Transform m_EyeLeft;

    [Header("Player Teleportation")]
    [SerializeField] private LayerMask m_CollidingLayers;
    [SerializeField] private Transform[] m_Controllers;
    [SerializeField] private Teleportable[] teleportables;
    [SerializeField] private LineRenderer m_LineRenderer;

    [Header("Movement")]
    [SerializeField] private float m_Movespeed = .05f;
    [SerializeField] private float m_MouseLookSpeed = 1f;

    public static float s_EyeSeperation = .05f;
    private Vector3 m_CurrentRotation = Vector3.zero;
    private Portal m_CurrentPortal;
    private XRInput m_Input;

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
        m_LineRenderer.enabled = false;
        m_Input = XRInput.Instance;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        s_EyeSeperation = Vector3.Distance(m_EyeRight.position, m_EyeLeft.position);

        if (m_IsDirectionallyControlled)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                m_IsMouseControlled = !m_IsMouseControlled;
            }

            if (!m_IsMouseControlled)
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
    }

    private bool m_IsTeleportGoalValid;
    private Vector3 m_TempTeleportGoalPos;
    private void FixedUpdate()
    {
        if(!m_IsDirectionallyControlled)
        {
            // left trigger
            if (m_Input.GetTrigger(true) 
                && Physics.Raycast(
                    new Ray(m_Controllers[0].position, m_Controllers[0].forward), 
                    out RaycastHit hit, 50f, m_CollidingLayers))
            {
                if(NavMesh.SamplePosition(hit.point, out _, .1f, NavMesh.AllAreas))
                {
                    m_IsTeleportGoalValid = true;
                    m_TempTeleportGoalPos = hit.point;

                    m_LineRenderer.SetPositions(new Vector3[] { m_Controllers[0].position, hit.point });
                    m_LineRenderer.enabled = true;
                }
                else
                {
                    m_IsTeleportGoalValid = false;

                    m_LineRenderer.enabled = false;
                }
            }
            else
            {
                // disable teleport line renderer
                m_LineRenderer.enabled = false;

                // teleport to selected position if its valid
                if (m_IsTeleportGoalValid)
                {
                    Debug.Log("REEEEEE");
                    transform.position = m_TempTeleportGoalPos;
                    m_IsTeleportGoalValid = false;
                }
            }
        }
    }

    private void UpdatePortalProcedures()
    {
        if (m_CurrentPortal == null) return;

        var relVec = m_CamHMD.transform.position - m_CurrentPortal.transform.position;
        var angle = Vector3.Angle(m_CurrentPortal.transform.forward, relVec);
        if (angle < 90f)
        {
            m_CurrentPortal.Mirror(transform, transform);

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
