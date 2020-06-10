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

    private MaterialPropertyBlock propertyBlock;

    protected virtual void Start()
    {
        InteractableManager.Instance.Add(this);
        gameObject.layer = InteractableManager.Instance.InteractableLayer;
        if (meshRenderer == null) meshRenderer = GetComponent<Renderer>();
    }

    public void SetHighlight(bool active)
    {
        if (Scannable)
        {
            meshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetInt("_Highlight", active ? 1 : 0);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }
    }

    public void SetOutline(bool active, bool ignoreOutlineOnHoverBool = false)
    {
        meshRenderer.GetPropertyBlock(propertyBlock);
        if (ignoreOutlineOnHoverBool || OutlineOnHover)
            propertyBlock.SetInt("_Outline", active ? 1 : 0);
        else 
            propertyBlock.SetInt("_Outline", 0);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }

    public abstract void EngageInteraction(Hand interactor);
    public abstract void DisengageInteraction(Hand interactor);
}
