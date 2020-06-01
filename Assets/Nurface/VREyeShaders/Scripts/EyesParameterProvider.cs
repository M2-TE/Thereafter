using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EyesParameterProvider : MonoBehaviour {

    [SerializeField] private bool m_IsMonoValue;
    [SerializeField] private Camera.MonoOrStereoscopicEye m_MonoValue;

    protected int parameterHashVector;
    protected int parameterHashFloat;
    public Vector4 leftEye = new Vector4(0.0f,0.0f,1.0f,0.5f);
    public Vector4 rightEye = new Vector4(0.0f,0.5f,1.0f,0.5f);
    protected Camera cam;

    private static bool s_OnRenderDelInitiated = false;

    static private void OnRender(ScriptableRenderContext ctx, Camera cam)
    {
        Debug.Log("INIT: " + cam.stereoTargetEye + cam.stereoActiveEye);
    }
    static private void PostRender(ScriptableRenderContext ctx, Camera cam)
    {
        Debug.Log("END: " + cam.stereoTargetEye + cam.stereoActiveEye);
    }

    void Awake()
    {
        if(!s_OnRenderDelInitiated)
        {
            //RenderPipelineManager.beginCameraRendering += OnRender;
            //RenderPipelineManager.endCameraRendering += PostRender;
            s_OnRenderDelInitiated = true;
        }

        parameterHashVector = Shader.PropertyToID("_EyeTransformVector");
        parameterHashFloat = Shader.PropertyToID("_EyeFloatFlag");
        cam = GetComponent<Camera>();
    }




    // added to work with our XR rig
    private void OnRenderObject()
    {
        //Debug.Log(cam.stereoActiveEye);
        Camera.MonoOrStereoscopicEye nextCamStereoEye = cam.stereoActiveEye;

        //Shader.SetGlobalVector(parameterHashVector, nextCamStereoEye == Camera.MonoOrStereoscopicEye.Left ? leftEye : rightEye);
        //Shader.SetGlobalFloat(parameterHashFloat, nextCamStereoEye == Camera.MonoOrStereoscopicEye.Left ? -1.0f : (nextCamStereoEye == Camera.MonoOrStereoscopicEye.Right ? 1.0f : 0.0f));



        //if (m_IsMonoValue)
        //{
        //    Shader.SetGlobalVector(parameterHashVector, m_MonoValue == Camera.MonoOrStereoscopicEye.Left ? rightEye : leftEye); // swapped left with right to adjust to "post"-render, not pre
        //    Shader.SetGlobalFloat(parameterHashFloat, m_MonoValue == Camera.MonoOrStereoscopicEye.Left ? 1.0f : (m_MonoValue == Camera.MonoOrStereoscopicEye.Right ? -1.0f : 0.0f)); // same here
        //}
        //else
        //{

        //}
    }
}
