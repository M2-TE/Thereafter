using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimationTrigger : MonoBehaviour
{
    public Animator LeftHand, RightHand;

    [SerializeField]
    private bool isClosedLeft = true, isClosedRight = true;

    void Update()
    {
        // Triggers Hand animation on Grip Press
        if(isClosedLeft != XRInput.Instance.GetGrip(true))
        {
            isClosedLeft = !isClosedLeft;
            ToggleHandState(isClosedLeft, true);
        }
        if(isClosedRight != XRInput.Instance.GetGrip(false))
        {
            isClosedRight = !isClosedRight;
            ToggleHandState(isClosedRight, false);
        }

        //DEBUG KeyCodes
        //if(Input.GetKeyDown(KeyCode.Space)) ToggleHandState(true,true);
        //if(Input.GetKeyDown(KeyCode.E)) ToggleHandState(false, true);
        //if (Input.GetKeyUp(KeyCode.E)) ToggleHandState(true, false);
        //if (Input.GetKeyUp(KeyCode.Space)) ToggleHandState(false, false);
    }

    public void ToggleHandState(bool closed, bool leftHand)
    {
        if (leftHand)
            LeftHand.SetBool("isOpen", !closed);
        else
            RightHand.SetBool("isOpen", !closed);
    }


}
