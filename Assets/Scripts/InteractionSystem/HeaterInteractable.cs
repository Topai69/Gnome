using UnityEngine;

public class HeaterInteractable : InteractableBase
{
    [SerializeField] private AnimationClip rotation;
    private Animation anim;

    /*private void Start()
    {
        anim = transform.parent.GetComponent<Animation>();
        anim.AddClip(rotation, "Rotating");
    }*/

    public override void OnInteract()
    {
        base.OnInteract();
        Debug.Log("Interacted with heater");
        //anim.Play("Rotating");
        (transform.parent.gameObject.GetComponent<Animation>()).Play("Activation");
    }
}