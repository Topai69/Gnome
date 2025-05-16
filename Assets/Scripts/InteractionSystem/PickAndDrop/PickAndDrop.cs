using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickAndDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform itemGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    
    [Header("Interazione")]
    [SerializeField] private InteractionInputData interactionInputData;
    [SerializeField] private KeyCode dropKey = KeyCode.Q;
    [SerializeField] private bool useInteractionSystem = true;
    
    private ItemGrabbable itemGrabbable;

    private void Update()
    {
        if (!useInteractionSystem)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (itemGrabbable == null)
                {
                    AttemptPickup();
                }
                else
                {
                    DropItem();
                }
            }
        }
        
        if (itemGrabbable != null && Input.GetKeyDown(dropKey))
        {
            DropItem();
        }
    }
    
    private void AttemptPickup()
    {
        float pickUpDistance = 2f;
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ItemGrabbable grabbable))
            {
                GrabItem(grabbable);
            }
        }
    }
    
    private void GrabItem(ItemGrabbable grabbable)
    {
        itemGrabbable = grabbable;
        itemGrabbable.Grab(itemGrabPointTransform);
    }
    
    private void DropItem()
    {
        if (itemGrabbable != null)
        {
            itemGrabbable.Drop();
            itemGrabbable = null;
        }
    }
    
    public void HandleGrabbableInteraction(ItemGrabbable interactedItem)
    {
        if (itemGrabbable == null)
        {
            GrabItem(interactedItem);
        }
        else if (itemGrabbable == interactedItem)
        {
            DropItem();
        }
    }

    public bool IsHoldingItem()
    {
        return itemGrabbable != null;
    }
    
    public ItemGrabbable GetHeldItem()
    {
        return itemGrabbable;
    }
}