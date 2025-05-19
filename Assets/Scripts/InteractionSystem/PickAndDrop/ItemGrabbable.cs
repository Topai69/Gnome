using UnityEngine;

public class ItemGrabbable : InteractableBase
{
    [Header("Grab Settings")]
    [SerializeField] private float lerpSpeed = 10f;
    [SerializeField] private bool canBeDropped = true;

    [SerializeField] private TrashType trashType;
    public TrashType GetTrashType() => trashType;
    
    private Rigidbody itemRigidbody;
    private Transform itemGrabPointTransform;
    private bool isGrabbed = false;
    private PickAndDrop pickAndDropSystem;
    private bool wrongBinPopupShown = false;

    private void Awake()
    {
        itemRigidbody = GetComponent<Rigidbody>();
        if (itemRigidbody == null)
        {
            itemRigidbody = gameObject.AddComponent<Rigidbody>();
        }
    
        isInteractable = true;
        holdInteract = false;  
        multipleUse = true;    
    }
    
    public override void OnInteract()
    {
        if (pickAndDropSystem == null)
        {
            pickAndDropSystem = FindObjectOfType<PickAndDrop>();
            if (pickAndDropSystem == null)
            {
                Debug.LogError("erorr");
                return;
            }
        }
       
        pickAndDropSystem.HandleGrabbableInteraction(this);
    }

    public void Grab(Transform itemGrabPointTransform)
    {
        this.itemGrabPointTransform = itemGrabPointTransform;
        itemRigidbody.useGravity = false;
        itemRigidbody.linearDamping = 10f;  
        isGrabbed = true;
    }

    public void Drop()
    {
        if (!canBeDropped)
            return;
            
        this.itemGrabPointTransform = null;
        itemRigidbody.useGravity = true;
        itemRigidbody.linearDamping = 0f;  
        isGrabbed = false;
    }
    
    public bool IsGrabbed()
    {
        return isGrabbed;
    }
    
    public bool HasShownWrongBinPopup()
    {
        return wrongBinPopupShown;
    }
    
    public void SetWrongBinPopupShown(bool value)
    {
        wrongBinPopupShown = value;
    }
    
    private void FixedUpdate()
    {
        if (itemGrabPointTransform != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, itemGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            itemRigidbody.MovePosition(newPosition);
        }
    }
}