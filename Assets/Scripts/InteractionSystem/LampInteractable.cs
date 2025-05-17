using UnityEngine;

public class LampInteractable : InteractableBase
{
    

    /*private void Start()
    {
        anim = transform.parent.GetComponent<Animation>();
        anim.AddClip(rotation, "Rotating");
    }*/

    public override void OnInteract()
    {
        base.OnInteract();
        Debug.Log("Interacted with lamp");
        //anim.Play("Rotating");
        
    }
}