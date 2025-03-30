using UnityEngine;

public class PlugInteractable : InteractableBase
{
    [SerializeField] private Vector3 targetPosition = new Vector3(-30.2130966f, 0.381148338f, 18.9699993f);

    public override void OnInteract(){
        base.OnInteract();
        Debug.Log("Interacted with plug: " + gameObject.name);
        transform.position = targetPosition;
    }
}