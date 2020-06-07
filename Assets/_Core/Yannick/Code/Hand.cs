using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Animator handAnimator;

    [SerializeField]
    private bool openHand;

    public void ToggleAnimation(bool openHand)
    {
        if(this.openHand != openHand)
        {
            this.openHand = openHand;
            handAnimator.SetBool("isOpen", openHand);
        }
    }
}
