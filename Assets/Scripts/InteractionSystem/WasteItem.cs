using UnityEngine;

public enum WasteType
{
    Organic,
    Glass,
    Paper,
    Plastic
}

public class WasteItem : MonoBehaviour
{
    [SerializeField] private WasteType wasteType;
    [SerializeField] private GameObject visualFeedback;
    
    private bool isBeingHeld = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;

    public WasteType Type => wasteType;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalParent = transform.parent;
        
        if (visualFeedback != null)
            visualFeedback.SetActive(false);
    }

    public void PickUp(Transform holder)
    {
        isBeingHeld = true;
        transform.SetParent(holder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        isBeingHeld = false;
        transform.SetParent(originalParent);
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    public void ShowInvalidPlacement()
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
}