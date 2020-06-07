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

    public void PickUp(Transform holder)
    {
        isPickedUp = true;
        OutlineOnHover = false;

        transform.parent = holder;

        rbody.isKinematic = true;
    }

    public void DropDown()
    {
        isPickedUp = false;
        OutlineOnHover = true;

        transform.parent = null;

        rbody.isKinematic = false;
    }
}
