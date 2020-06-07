using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInteractable : Interactable
{
    // Interaction, where the Hand attaches to the object
    public override void DisengageInteraction(Hand interactor)
    {
        throw new System.NotImplementedException();
    }

    public override void EngageInteraction(Hand interactor)
    {
        throw new System.NotImplementedException();
    }
}
