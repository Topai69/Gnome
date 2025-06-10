using UnityEngine;

public class ItemGrabbable : InteractableBase
{
    [Header("Grab Settings")]
    [SerializeField] private float lerpSpeed = 10f;
    [SerializeField] private bool canBeDropped = true;

    [SerializeField] private TrashType trashType;
    public TrashType GetTrashType() => trashType;

    private Rigidbody itemRigidbody;
    private Transform grabPointTransform;
    private GameObject grabPointObject;
    private bool isGrabbed = false;

    private Camera mainCamera;
    private InteractionUIPanel interactionUI;

    private Quaternion originalRotation;
    private bool wrongBinPopupShown = false;

    private static string[] noCollisionNames = { "Bottle", "Paper2", "plasticCup", "Battery" };

    private void Awake()
    {
        itemRigidbody = GetComponent<Rigidbody>();
        if (itemRigidbody == null)
        {
            itemRigidbody = gameObject.AddComponent<Rigidbody>();
        }

        mainCamera = Camera.main;

        isInteractable = true;
        holdInteract = false;
        multipleUse = true;
    }

    private void Start()
    {
        interactionUI = FindObjectOfType<InteractionUIPanel>();

        foreach (string name in noCollisionNames)
        {
            if (gameObject.name.Contains(name))
            {
                DisableCollisionsWithPlayer();
                DisableCollisionsWithOtherTrashObjects();
                break;
            }
        }
    }

    private void DisableCollisionsWithPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Collider thisCol = GetComponent<Collider>();
        foreach (Collider playerCol in player.GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(thisCol, playerCol, true);
        }
    }

    private void DisableCollisionsWithOtherTrashObjects()
    {
        Collider thisCol = GetComponent<Collider>();
        ItemGrabbable[] allTrash = FindObjectsOfType<ItemGrabbable>();

        foreach (ItemGrabbable other in allTrash)
        {
            if (other != this)
            {
                foreach (string name in noCollisionNames)
                {
                    if (other.name.Contains(name))
                    {
                        Collider otherCol = other.GetComponent<Collider>();
                        if (thisCol != null && otherCol != null)
                        {
                            Physics.IgnoreCollision(thisCol, otherCol, true);
                        }
                    }
                }
            }
        }
    }

    public override void OnInteract()
    {
        if (!isGrabbed)
        {
            Grab();
        }
        else if (canBeDropped)
        {
            Drop();
        }
    }

    private void Grab()
    {
        if (grabPointObject == null)
        {
            grabPointObject = new GameObject("GrabPoint");
        }

        grabPointTransform = grabPointObject.transform;
        grabPointTransform.SetParent(mainCamera.transform);
        grabPointTransform.localPosition = new Vector3(-0.01f, 0.01f, 7f);
        grabPointTransform.localRotation = Quaternion.identity;

        originalRotation = transform.rotation;

        itemRigidbody.useGravity = false;
        itemRigidbody.linearDamping = 5f;
        itemRigidbody.freezeRotation = true;

        isGrabbed = true;

        if (interactionUI != null)
        {
            interactionUI.ResetUI();
            interactionUI.gameObject.SetActive(false);
        }

        if (gameObject.name.Contains("Battery"))
        {
            Invoke(nameof(ForceReleaseAndDestroy), 2f);
        }
    }

    private void Drop()
    {
        isGrabbed = false;

        itemRigidbody.useGravity = true;
        itemRigidbody.linearDamping = 0f;
        itemRigidbody.freezeRotation = false;

        if (grabPointTransform != null)
        {
            grabPointTransform.SetParent(null);
            grabPointTransform = null;
        }

        if (interactionUI != null)
        {
            interactionUI.gameObject.SetActive(true);
            interactionUI.SetTooltip("Interact");
            interactionUI.UpdateProgressBar(0f);
        }
    }

    private void ForceReleaseAndDestroy()
    {
        isGrabbed = false;

        if (grabPointTransform != null)
        {
            grabPointTransform.SetParent(null);
            grabPointTransform = null;
        }

        itemRigidbody.useGravity = true;
        itemRigidbody.linearDamping = 0f;
        itemRigidbody.freezeRotation = false;

        if (interactionUI != null)
        {
            interactionUI.gameObject.SetActive(true);
            interactionUI.SetTooltip("Interact");
            interactionUI.UpdateProgressBar(0f);
        }

        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (isGrabbed && grabPointTransform != null)
        {
            Vector3 targetPos = grabPointTransform.position;
            Vector3 newPos = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * lerpSpeed);
            itemRigidbody.MovePosition(newPos);

            itemRigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, originalRotation, Time.fixedDeltaTime * lerpSpeed));
        }
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
