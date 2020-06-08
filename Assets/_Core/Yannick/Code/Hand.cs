using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hand : MonoBehaviour
{
    public Animator handAnimator;
    public bool Left => leftHand;

    [SerializeField]
    private bool openHand, leftHand;

    private bool isInteracting = false;
    private Interactable reachAble = null;

    private void Update()
    {
        if (XRInput.Instance.GetGrip(leftHand) != isInteracting)
        {
            isInteracting = !isInteracting;
            Interact(isInteracting);
        }
    }

    public void ToggleAnimation(bool openHand)
    {
        if(this.openHand != openHand)
        {
            this.openHand = openHand;
            handAnimator.SetBool("isOpen", openHand);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Interactable>())
        {
            reachAble = other.GetComponent<Interactable>();
            reachAble.SetOutline(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (reachAble == null && other.GetComponent<Interactable>()) reachAble = other.GetComponent<Interactable>();
        reachAble.SetOutline(false);
        reachAble = null;
    }

    /// <param name="engage">TRUE if the interaction has started. FALSE if it has ended.</param>
    public void Interact(bool engage)
    {
        if(reachAble != null)
        {
            if (engage)
                reachAble.EngageInteraction(this);
            else
                reachAble.DisengageInteraction(this);
        }
        ToggleAnimation(!engage);
    }
}
