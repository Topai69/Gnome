using UnityEngine;

public class PlugInteractable : InteractableBase
{
    [SerializeField] private Vector3 targetPosition = new Vector3(-9.3f, -10.36f, 102.5f);

    public override void OnInteract()
    {
        base.OnInteract();
        Debug.Log("Interacted with plug: " + gameObject.name);
        transform.position = targetPosition;
    }
}
