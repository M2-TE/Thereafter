using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Interactable : MonoBehaviour
{
    [SerializeField][Range(0.2f, 2f)] private float  interactRange = 1f;
    [SerializeField] private bool highlightOnHover = true;
 
}
