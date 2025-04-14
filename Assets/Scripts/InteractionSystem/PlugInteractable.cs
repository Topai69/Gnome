using UnityEngine;

public class PlugInteractable : InteractableBase
{
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private GameObject light;
    public override void OnInteract(){
        base.OnInteract();
        Debug.Log("Interacted with plug: " + gameObject.name);
        transform.position = targetPosition;
        light.GetComponent<Light>().intensity = 0;
    }
}