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

    private Quaternion originalRotation;

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
        
        itemRigidbody.freezeRotation = true;
        originalRotation = transform.rotation;
        
        isGrabbed = true;
    }

    private void FixedUpdate()
    {
        if (itemGrabPointTransform != null && isGrabbed)
        {
            Vector3 targetPosition = itemGrabPointTransform.position;
            Vector3 lerpedPosition = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * lerpSpeed);
            itemRigidbody.MovePosition(lerpedPosition);
            
            Quaternion targetRotation = originalRotation; 
            itemRigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * lerpSpeed));
        }
    }

    public void Drop()
    {
        if (!canBeDropped)
            return;
            
        this.itemGrabPointTransform = null;
        itemRigidbody.useGravity = true;
        itemRigidbody.linearDamping = 0f;
        itemRigidbody.freezeRotation = false;
        
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
}
