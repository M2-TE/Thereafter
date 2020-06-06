using UnityEngine;

[DefaultExecutionOrder(0)]
public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject m_QuadDisplayRight;
    [SerializeField] private GameObject m_QuadDisplayLeft;
    private GameObject m_QuadCopyRight;
    private GameObject m_QuadCopyLeft;

    public Portal m_Pair;

    /// <summary>
    /// //////////
    /// </summary>
    /// 

    [SerializeField] private Camera m_EyeLeft;
    [SerializeField] private Camera m_EyeRight;
    private Camera m_MainCam;

    private void Awake()
    {
        m_QuadCopyLeft = Instantiate(m_QuadDisplayLeft, transform);
        m_QuadCopyRight = Instantiate(m_QuadDisplayRight, transform);
    }

    private void Update()
    {
        Vector3 vec = transform.forward * PlayerController.s_EyeSeperation + transform.position;
        m_QuadCopyLeft.transform.position = vec;
        m_QuadCopyRight.transform.position = vec;
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


    ////////////////////
    ///
    private void OnEnable()
    {
        Application.onBeforeRender += OnBeforeRender;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= OnBeforeRender;
    }

    private void Start()
    {
        m_MainCam = Camera.main;
    }

    private void OnBeforeRender()
    {
        // render manually
        RenderLeft();
        RenderRight();

        //m_MainCam.Render();
    }

    private void RenderLeft(bool adjustPos = true)
    {
        if (adjustPos)
        {
            MirrorPosition(m_MainCam.transform, m_EyeLeft.transform);
            MirrorRotation(m_MainCam.transform, m_EyeLeft.transform);

            m_EyeLeft.projectionMatrix = m_MainCam.projectionMatrix;
            m_EyeLeft.transform.Translate(new Vector3(-PlayerController.s_EyeSeperation * .5f, 0f, 0f), Space.Self);
        }

        //SetActiveShaderEye(Camera.MonoOrStereoscopicEye.Left);
        m_EyeLeft.Render();
    }

    private void RenderRight(bool adjustPos = true)
    {
        if (adjustPos)
        {
            MirrorPosition(m_MainCam.transform, m_EyeRight.transform);
            MirrorRotation(m_MainCam.transform, m_EyeRight.transform);

            m_EyeRight.projectionMatrix = m_MainCam.GetStereoNonJitteredProjectionMatrix(Camera.StereoscopicEye.Right);
            m_EyeRight.transform.Translate(new Vector3(PlayerController.s_EyeSeperation * .5f, 0f, 0f), Space.Self);
        }

        //SetActiveShaderEye(Camera.MonoOrStereoscopicEye.Right);
        m_EyeRight.Render();
    }
}
