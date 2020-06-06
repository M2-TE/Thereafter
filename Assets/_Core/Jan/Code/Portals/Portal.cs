using UnityEngine;

[DefaultExecutionOrder(0)]
public class Portal : MonoBehaviour
{
    public Portal m_Pair;
    [Header("Prefabs")]
    [SerializeField] private GameObject m_camPrefab;
    [SerializeField] private RenderTexture m_renderTexturePrefab;
    [SerializeField] private GameObject m_portalQuadPrefab;

    private Camera m_mainCam;
    private Camera m_camLeft;
    private Camera m_camRight;

    private GameObject m_quadCopyRight;
    private GameObject m_quadCopyLeft;

    private void OnEnable()
    {
        Application.onBeforeRender += OnBeforeRender;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= OnBeforeRender;
    }

    private void Awake()
    {
        // left eye
        {
            var go = Instantiate(m_camPrefab, transform);
            m_camLeft = go.GetComponent<Camera>();
            m_camLeft.stereoTargetEye = StereoTargetEyeMask.Left;
            m_camLeft.targetTexture = new RenderTexture(m_renderTexturePrefab);

            var quadLeft = Instantiate(m_portalQuadPrefab, transform);
            quadLeft.name = "Quad Left";
            var mat = quadLeft.GetComponent<MeshRenderer>().material;
            mat.SetTexture("_PortalTexture", m_camLeft.targetTexture);
            mat.SetFloat("_TargetEye", -1f);

            // create quad copies to smoothen the teleportation visually
            m_quadCopyLeft = Instantiate(quadLeft, transform);
            m_quadCopyLeft.name = "Quad Left - Copy";
        }

        // right eye
        {
            var go = Instantiate(m_camPrefab, transform);
            m_camRight = go.GetComponent<Camera>();
            m_camRight.stereoTargetEye = StereoTargetEyeMask.Right;
            m_camRight.targetTexture = new RenderTexture(m_renderTexturePrefab);

            var quadRight = Instantiate(m_portalQuadPrefab, transform);
            quadRight.name = "Quad Right";
            var mat = quadRight.GetComponent<MeshRenderer>().material;
            mat.SetTexture("_PortalTexture", m_camRight.targetTexture);
            mat.SetFloat("_TargetEye", 1f);

            // create quad copies to smoothen the teleportation visually
            m_quadCopyRight = Instantiate(quadRight, transform);
            m_quadCopyRight.name = "Quad Right - Copy";
        }
    }

    private void Start()
    {
        m_mainCam = Camera.main;
    }

    private void Update()
    {
        Vector3 vec = transform.forward * PlayerController.s_EyeSeperation + transform.position;
        m_quadCopyLeft.transform.position = vec;
        m_quadCopyRight.transform.position = vec;
    }

    public void Mirror(Transform original, Transform target)
    {
        MirrorPosition(original, target);
        MirrorRotation(original, target);
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
            MirrorPosition(m_mainCam.transform, m_camLeft.transform);
            MirrorRotation(m_mainCam.transform, m_camLeft.transform);

            m_camLeft.projectionMatrix = m_mainCam.projectionMatrix;
            m_camLeft.transform.Translate(new Vector3(-PlayerController.s_EyeSeperation * .5f, 0f, 0f), Space.Self);
        }

        m_camLeft.Render();
    }

    private void RenderRight(bool adjustPos = true)
    {
        if (adjustPos)
        {
            MirrorPosition(m_mainCam.transform, m_camRight.transform);
            MirrorRotation(m_mainCam.transform, m_camRight.transform);

            m_camRight.projectionMatrix = m_mainCam.GetStereoNonJitteredProjectionMatrix(Camera.StereoscopicEye.Right);
            m_camRight.transform.Translate(new Vector3(PlayerController.s_EyeSeperation * .5f, 0f, 0f), Space.Self);
        }

        m_camRight.Render();
    }
}
