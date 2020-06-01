using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class PortalMain : PortalBase
{
    [SerializeField] private RenderTexture m_RtEyeLeft;
    [SerializeField] private RenderTexture m_RtEyeRight;
    [Space]
    [SerializeField] private Camera m_EyeLeft;
    [SerializeField] private Camera m_EyeRight;
    [SerializeField] private Transform m_EyeTransformLeft;
    [SerializeField] private Transform m_EyeTransformRight;
    private Camera m_MainCam;

    [Header("Cam Stuff - TODO")]
    protected int parameterHashVector;
    protected int parameterHashFloat;
    public Vector4 leftEye = new Vector4(0.0f, 0.0f, 1.0f, 0.5f);
    public Vector4 rightEye = new Vector4(0.0f, 0.5f, 1.0f, 0.5f);
    bool m_RightEyeQueued = false;

    private void Start()
    {
        parameterHashVector = Shader.PropertyToID("_EyeTransformVector");
        parameterHashFloat = Shader.PropertyToID("_EyeFloatFlag");
        m_MainCam = Camera.main;
        RenderPipelineManager.beginCameraRendering += DoStuff;
    }


    private void Update()
    {
        {
            m_EyeLeft.projectionMatrix = m_MainCam.projectionMatrix; // Match matrices
            m_EyeRight.projectionMatrix = m_MainCam.projectionMatrix; // Match matrices

            // left eye
            {
                var relativePosition = transform.InverseTransformPoint(m_EyeTransformLeft.position);
                relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
                m_EyeLeft.transform.position = m_Pair.transform.TransformPoint(relativePosition);


                var relativeRotation = transform.InverseTransformDirection(m_MainCam.transform.forward);
                relativeRotation = Vector3.Scale(relativeRotation, new Vector3(-1, 1, -1));
                m_EyeLeft.transform.forward = m_Pair.transform.TransformDirection(relativeRotation);
            }

            // right eye
            {
                var relativePosition = transform.InverseTransformPoint(m_EyeTransformRight.position);
                relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
                m_EyeRight.transform.position = m_Pair.transform.TransformPoint(relativePosition);


                var relativeRotation = transform.InverseTransformDirection(m_MainCam.transform.forward);
                relativeRotation = Vector3.Scale(relativeRotation, new Vector3(-1, 1, -1));
                m_EyeRight.transform.forward = m_Pair.transform.TransformDirection(relativeRotation);
            }
        }
    }

    private void DoStuff(ScriptableRenderContext ctx, Camera cam)
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
