using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimationTrigger : MonoBehaviour
{
    public Animator LeftHand, RightHand;

    [SerializeField]
    private bool isOpen = true;

    void Update()
    {




        if (Input.GetKeyDown(KeyCode.Space)) LeftHand.SetBool("isOpen", false);
        if(Input.GetKeyDown(KeyCode.E)) RightHand.SetBool("isOpen", false);
        if(Input.GetKeyUp(KeyCode.E)) RightHand.SetBool("isOpen", true);
        if (Input.GetKeyUp(KeyCode.Space)) LeftHand.SetBool("isOpen", true);
    }
}
