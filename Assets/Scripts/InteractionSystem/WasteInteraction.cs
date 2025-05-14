using UnityEngine;

public class WasteInteraction : InteractableBase
{
    [SerializeField] private WasteType wasteType;
    [SerializeField] private GameObject visualFeedback;
    [SerializeField] private Transform playerHoldPoint;
    
    private bool isBeingHeld = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;
    private WasteContainer currentContainer;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalParent = transform.parent;

        if (visualFeedback != null)
            visualFeedback.SetActive(false);
    }

    public override void OnInteract()
    {
        base.OnInteract();

        if (!isBeingHeld)
        {
            PickUp();
        }
        else
        {
            TryDrop();
        }
    }

    private void PickUp()
    {
        isBeingHeld = true;
        transform.SetParent(playerHoldPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void TryDrop()
    {
        Debug.Log($"[WasteInteraction] {gameObject.name} TryDrop. CurrentContainer: {currentContainer?.name}");
        if (currentContainer != null)
        {
            bool canAccept = currentContainer.CanAcceptItem(this);
            Debug.Log($"[WasteInteraction] {gameObject.name} Container {currentContainer.name} - can accept {canAccept}");
            if (canAccept)
            {
                PlaceInContainer(currentContainer);
            }
            else
            {
                Debug.Log($"[WasteInteraction] {gameObject.name} invalid container {currentContainer.name}");
                ShowInvalidPlacement();
            }
        }
        else
        {
            Debug.Log($"[WasteInteraction] {gameObject.name} No container in range, back to original pos");
            DropToOriginalPosition();
        }
    }

    private void PlaceInContainer(WasteContainer container)
    {
        Debug.Log($"[WasteInteraction] {gameObject.name} PlaceInContainer: {container.name}. DropPoint: {container.DropPoint?.name}");
        isBeingHeld = false;

        if (container.DropPoint != null)
        {
            transform.SetParent(container.DropPoint); 
            transform.localPosition = Vector3.zero;   
            transform.localRotation = Quaternion.identity; 
        }
        else
        {
            transform.SetParent(container.transform); 
            transform.position = container.transform.position; 
        }

        InteractableBase interactable = GetComponent<InteractableBase>();
        if (interactable != null)
        {
            interactable.isInteractable = false; 
        }
    }

    private void DropToOriginalPosition() 
    {
        Debug.Log($"[WasteInteraction] {gameObject.name} DropToOriginalPosition.");
        isBeingHeld = false;
        transform.SetParent(originalParent);
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    private void ShowInvalidPlacement()
    {
        if (visualFeedback != null)
        {
            visualFeedback.SetActive(true);
            StartCoroutine(HideFeedbackAfterDelay());
        }
    }

    private System.Collections.IEnumerator HideFeedbackAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        if (visualFeedback != null)
            visualFeedback.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        WasteContainer container = other.GetComponent<WasteContainer>();
        if (container != null)
        {
            Debug.Log($"[WasteInteraction] {gameObject.name} OnTriggerEnter with {other.name}. Container: {container.name}");
            currentContainer = container;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WasteContainer container = other.GetComponent<WasteContainer>();
        if (container != null && container == currentContainer)
        {
            Debug.Log($"[WasteInteraction] {gameObject.name} OnTriggerExit from {other.name}. CurrentContainer: {currentContainer.name}");
            currentContainer = null;
        }
    }

    public WasteType Type => wasteType;
}