using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public static InteractableManager Instance => instance;

    private static InteractableManager instance;

    public Transform InteractablesParent;

    public int InteractableLayer = 15;

    private List<Interactable> allInteractables = new List<Interactable>();


    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) Scanner(true);
        if (Input.GetKeyDown(KeyCode.U)) Scanner(false);
    }

    public void Add(Interactable interactable)
    {
        allInteractables.Add(interactable);
    }

    public void Scanner(bool active)
    {
        foreach (var interactable in allInteractables)
        {
            interactable.SetHighlight(active);
        }
    }

}
