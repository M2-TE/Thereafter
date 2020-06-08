using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PickUpInteractable : Interactable
{
    public bool IsPickedUp => isPickedUp;

    private bool isPickedUp = false;

    private Rigidbody rbody;

    protected override void Start()
    {
        base.Start();
        rbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// PickUp Event
    /// </summary>
    /// <param name="holder">The Hand the interactable will attach itself to.</param>
    public override void EngageInteraction(Hand holder)
    {
        isPickedUp = true;
        OutlineOnHover = false;

        transform.parent = holder.transform;

        rbody.isKinematic = true;
    }

    /// <summary>
    /// DropDown Event
    /// </summary>
    /// <param name="holder">The Hand the interactable will detach itself from.</param>
    public override void DisengageInteraction(Hand holder)
    {
        isPickedUp = false;
        OutlineOnHover = true;

        transform.parent = InteractableManager.Instance.InteractablesParent;

        rbody.isKinematic = false;

        Vector3 velocity = XRInput.Instance.GetControllerVelocity(holder.Left);

        // the X and Z Velocity from the controllers are inverted for whatever reason
        rbody.velocity = new Vector3(-velocity.x,velocity.y,-velocity.z);
    }
}
