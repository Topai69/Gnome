using UnityEngine;

public class FridgeInteractable : InteractableBase
{
    /*private void Start()
    {
        anim = transform.parent.GetComponent<Animation>();
        anim.AddClip(rotation, "Rotating");
    }*/

    public override void OnInteract()
    {
        base.OnInteract();
        Debug.Log("Interacted with fridge");
        //anim.Play("Rotating");
        (transform.parent.gameObject.GetComponent<Animation>()).Play("Fridge");
    }
}