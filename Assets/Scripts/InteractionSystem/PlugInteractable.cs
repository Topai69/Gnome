using UnityEngine;

public class PlugInteractable : InteractableBase
{
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private GameObject Light;

    public override void OnInteract(){
        base.OnInteract();
        Debug.Log("Interacted with plug: " + gameObject.name);
        transform.parent.position = targetPosition;
        Light.GetComponent<Light>().intensity = 0;
    }
}