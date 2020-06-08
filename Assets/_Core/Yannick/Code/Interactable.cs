using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public abstract class Interactable : MonoBehaviour
{
    [Tooltip("References the Renderer automatically if not set here.")]
    [SerializeField] private Renderer meshRenderer;

    public bool OutlineOnHover = true;
    public bool Scannable = true;

    private void Awake()
    {
        if(meshRenderer == null) meshRenderer = GetComponent<Renderer>();
        meshRenderer.material = new Material(meshRenderer.material);
    }

    protected virtual void Start()
    {
        InteractableManager.Instance.Add(this);
        gameObject.layer = InteractableManager.Instance.InteractableLayer;
    }

    public void SetHighlight(bool active)
    {
        if(Scannable)
            meshRenderer.material.SetInt("_Highlight", active ? 1 : 0);
    }

    public void SetOutline(bool active, bool ignoreOutlineOnHoverBool = false)
    {
        if (ignoreOutlineOnHoverBool || OutlineOnHover)
            meshRenderer.material.SetInt("_Outline", active ? 1 : 0);
        else 
            meshRenderer.material.SetInt("_Outline", 0);
    }

    public abstract void EngageInteraction(Hand interactor);
    public abstract void DisengageInteraction(Hand interactor);
}
