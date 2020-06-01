using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Test : MonoBehaviour
{
    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += UpdateCamera;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= UpdateCamera;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Debug.Log("yay");
    }

    private void UpdateCamera(ScriptableRenderContext context, Camera cam)
    {
        //Debug.Log(cam.SetTargetBuffers());
        //Debug.Log(cam.targetTexture);
        //cam.stereoTargetEye = StereoTargetEyeMask.Left;
        //cam.stereoTargetEye = StereoTargetEyeMask.Right;
        //Debug.Log(cam.stereoTargetEye);
        //Debug.Log(cam.stereoEnabled);
        //Debug.Log(cam.name);
        //context.StartMultiEye(cam, 0);
        //Debug.Log(cam.stereoActiveEye);
    }

    private void Update()
    {
        //Debug.Log("Update");
    }
}
