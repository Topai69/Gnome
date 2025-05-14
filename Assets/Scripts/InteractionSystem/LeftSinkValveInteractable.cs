using UnityEngine;

public class LeftSinkValveInteractable : InteractableBase
{
    /*private void Start()
    {
        anim = transform.parent.GetComponent<Animation>();
        anim.AddClip(rotation, "Rotating");
    }*/

    public override void OnInteract()
    {
        base.OnInteract();
        Debug.Log("Interacted with left sink valve");
        //anim.Play("Rotating");
        (transform.parent.gameObject.GetComponent<Animation>()).Play("LeftSinkValve");
    }
}