using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    bool m_rightEyeQueued = false;
    protected int parameterHashVector;
    protected int parameterHashFloat;
    public Vector4 leftEye = new Vector4(0.0f, 0.0f, 1.0f, 0.5f);
    public Vector4 rightEye = new Vector4(0.0f, 0.5f, 1.0f, 0.5f);

    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRender;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRender;
    }

    private void Start()
    {
        parameterHashVector = Shader.PropertyToID("_EyeTransformVector");
        parameterHashFloat = Shader.PropertyToID("_EyeFloatFlag");
    }

    private void OnBeginCameraRender(ScriptableRenderContext ctx, Camera cam)
    {
        switch (cam.stereoTargetEye)
        {
            case StereoTargetEyeMask.Both:
                {
                    SetActiveShaderEye(Camera.MonoOrStereoscopicEye.Left);
                    m_rightEyeQueued = true;
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
        if (m_rightEyeQueued)
        {
            SetActiveShaderEye(Camera.MonoOrStereoscopicEye.Right);
            m_rightEyeQueued = false;
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
