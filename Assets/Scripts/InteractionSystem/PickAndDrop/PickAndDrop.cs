using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickAndDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform playerTransform; 
    [SerializeField] private Vector3 grabPointOffset = new Vector3(0.0f, 0.0f, 1.5f); 
    private Transform dynamicGrabPoint;
    [SerializeField] private LayerMask pickUpLayerMask;
    
    [Header("Interazione")]
    [SerializeField] private InteractionInputData interactionInputData;
    [SerializeField] private KeyCode dropKey = KeyCode.Q;
    [SerializeField] private bool useInteractionSystem = true;
    [SerializeField] private float adjustmentSpeed = 0.1f;
    
    private ItemGrabbable itemGrabbable;

    private void Start()
    {
        if (playerTransform == null)
            playerTransform = transform;

        GameObject grabPointObj = new GameObject("GrabPoint");
        dynamicGrabPoint = grabPointObj.transform;
        dynamicGrabPoint.SetParent(playerTransform); 
 
        UpdateGrabPointPosition();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1)) grabPointOffset.x -= adjustmentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Alpha2)) grabPointOffset.x += adjustmentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Alpha3)) grabPointOffset.y -= adjustmentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Alpha4)) grabPointOffset.y += adjustmentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Alpha5)) grabPointOffset.z -= adjustmentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Alpha6)) grabPointOffset.z += adjustmentSpeed * Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.P))
            Debug.Log($" grab point offset: {grabPointOffset}");
        
        UpdateGrabPointPosition();

        if (Input.GetKeyDown(dropKey) && itemGrabbable != null)
        {
            DropItem();
        }
    }
    
    private void UpdateGrabPointPosition()
    {
        if (dynamicGrabPoint != null && playerCameraTransform != null)
        {
            Vector3 cameraForward = playerCameraTransform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            Vector3 basePosition = playerTransform.position + cameraForward * grabPointOffset.z;
        
            basePosition += Vector3.up * grabPointOffset.y;
            basePosition += playerTransform.right * grabPointOffset.x;

            dynamicGrabPoint.position = basePosition;
            
            dynamicGrabPoint.rotation = Quaternion.Euler(0, playerCameraTransform.eulerAngles.y, 0);
        }
    }

    public void HandleGrabbableInteraction(ItemGrabbable interactedItem)
    {
        Debug.Log($" interaction with {interactedItem.gameObject.name}");
        
        if (itemGrabbable == null)
        {
            GrabItem(interactedItem);
        }
        else if (itemGrabbable == interactedItem)
        {
            DropItem();
        }
    }

    private void GrabItem(ItemGrabbable grabbable)
    {
        Debug.Log($"grabbing {grabbable.gameObject.name}");
        
        itemGrabbable = grabbable;
        itemGrabbable.Grab(dynamicGrabPoint);
    }

    private void DropItem()
    {
        if (itemGrabbable != null)
        {
            Debug.Log($"dropping {itemGrabbable.gameObject.name}");
            
            itemGrabbable.Drop();
            itemGrabbable = null;
        }
    }
}