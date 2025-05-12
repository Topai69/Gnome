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
        if (currentContainer != null)
        {
            if (currentContainer.CanAcceptItem(this))
            {
                Drop();
            }
            else
            {
                ShowInvalidPlacement();
            }
        }
        else
        {
            Drop();
        }
    }

    private void Drop()
    {
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
            currentContainer = container;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WasteContainer container = other.GetComponent<WasteContainer>();
        if (container == currentContainer)
        {
            currentContainer = null;
        }
    }

    public WasteType Type => wasteType;
}