using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Interactable : MonoBehaviour
{
    [SerializeField][Range(0.2f, 2f)] private float  interactRange = 1f;
    [SerializeField] private bool highlightOnHover = true;
    [SerializeField] private Material highlightMat;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = highlightMat;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) highlightMat.SetInt("_Highlight", 1);
        if (Input.GetKeyDown(KeyCode.E)) highlightMat.SetInt("_Outline", 1);
        if (Input.GetKeyUp(KeyCode.Space)) highlightMat.SetInt("_Highlight", 0);
        if (Input.GetKeyUp(KeyCode.E)) highlightMat.SetInt("_Outline", 0);
    }
}
