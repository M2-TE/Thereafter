using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SpatialTracking;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool m_IsMouseControlled;
    [Header("Trackers and Cams")]
    [SerializeField] private Camera m_CamHMD;
    [SerializeField] private Transform m_EyeLeft;
    [SerializeField] private Transform m_EyeRight;
    [SerializeField] private Transform m_ControllerLeft;
    [SerializeField] private Transform m_ControllerRight;

    [Header("Movement")]
    [SerializeField] private float m_Movespeed = .05f;
    [SerializeField] private float m_MouseLookSpeed = 1f;

    private Vector3 m_CurrentRotation = Vector3.zero;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
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
        var moveVec = new Vector3(hInput, 0f, vInput);
        moveVec = Quaternion.Euler(0f, m_CurrentRotation.y, 0f) * moveVec;
        transform.Translate(moveVec.normalized * m_Movespeed);
    }
}
