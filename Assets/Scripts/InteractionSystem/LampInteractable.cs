using UnityEngine;

public class LampInteractable : InteractableBase
{
    [SerializeField] private AnimationClip unplugging;
    private Animation anim;

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
        (transform.parent.gameObject.GetComponent<Animation>()).Play("Unplug");
    }
}