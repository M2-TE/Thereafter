using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCullingManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private float speed = 1.0f, minDistance = 0.02f, maxDistance = 120f;

    public bool increaseCulling = true;

    // Start is called before the first frame update
    void Start()
    {
        mainCam.farClipPlane = minDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (increaseCulling && mainCam.farClipPlane <= maxDistance)
        {
            mainCam.farClipPlane = Mathf.Min(mainCam.farClipPlane + Time.deltaTime * speed, maxDistance);
        }
        else
        {
            mainCam.farClipPlane = Mathf.Max(mainCam.farClipPlane - Time.deltaTime * speed,minDistance);
        }
    }
}
