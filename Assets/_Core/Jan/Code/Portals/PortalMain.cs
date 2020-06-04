using UnityEngine;
using UnityEngine.Rendering;

public class PortalMain : PortalBase
{
    //[SerializeField] private float m_EyeDist = .065f;
    [SerializeField] private RenderTexture m_RtEyeLeft;
    [SerializeField] private RenderTexture m_RtEyeRight;
    [Space]
    [SerializeField] private Camera m_EyeLeft;
    [SerializeField] private Camera m_EyeRight;
    private Camera m_MainCam;

    //

    [Header("Cam Stuff - TODO")]
    protected int parameterHashVector;
    protected int parameterHashFloat;
    public Vector4 leftEye = new Vector4(0.0f, 0.0f, 1.0f, 0.5f);
    public Vector4 rightEye = new Vector4(0.0f, 0.5f, 1.0f, 0.5f);
    bool m_RightEyeQueued = false;

    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRender;
        Application.onBeforeRender += OnBeforeRender;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRender;
        Application.onBeforeRender -= OnBeforeRender;
    }

    private void Start()
    {
        parameterHashVector = Shader.PropertyToID("_EyeTransformVector");
        parameterHashFloat = Shader.PropertyToID("_EyeFloatFlag");
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
        if(adjustPos)
        {
            MirrorPosition(m_MainCam.transform, m_EyeLeft.transform);
            MirrorRotation(m_MainCam.transform, m_EyeLeft.transform);

            m_EyeLeft.projectionMatrix = m_MainCam.projectionMatrix;
            m_EyeLeft.transform.Translate(new Vector3(-PlayerController.s_EyeSeperation * .5f, 0f, 0f), Space.Self);
        }

        SetActiveShaderEye(Camera.MonoOrStereoscopicEye.Left);
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

        SetActiveShaderEye(Camera.MonoOrStereoscopicEye.Right);
        m_EyeRight.Render();
    }

    private void OnBeginCameraRender(ScriptableRenderContext ctx, Camera cam)
    {
        switch (cam.stereoTargetEye)
        {
            case StereoTargetEyeMask.Both:
                {
                    SetActiveShaderEye(Camera.MonoOrStereoscopicEye.Left);
                    m_RightEyeQueued = true;
                }
                break;

            case StereoTargetEyeMask.Left:
                {
                    SetActiveShaderEye(Camera.MonoOrStereoscopicEye.Left);
                }
                break;
            case StereoTargetEyeMask.Right:
                {
                    SetActiveShaderEye(Camera.MonoOrStereoscopicEye.Right);
                }
                break;

            case StereoTargetEyeMask.None:
                {
                    Debug.LogError("wtf");
                }
                break;
        }
    }

    private void OnRenderObject()
    {
        if (m_RightEyeQueued)
        {
            SetActiveShaderEye(Camera.MonoOrStereoscopicEye.Right);
            m_RightEyeQueued = false;
        }
    }

    private void SetActiveShaderEye(Camera.MonoOrStereoscopicEye eye)
    {
        Shader.SetGlobalVector(parameterHashVector, eye == Camera.MonoOrStereoscopicEye.Left ? leftEye : rightEye);
        Shader.SetGlobalFloat(
            parameterHashFloat, eye == Camera.MonoOrStereoscopicEye.Left ? 
            -1.0f : 
            (eye == Camera.MonoOrStereoscopicEye.Right ? 
                1.0f : 
                0.0f));
    }
}
