using UnityEngine;

public class WasteInteraction : InteractableBase
{
    [SerializeField] private WasteItem wasteItem;
    [SerializeField] private Transform holdPoint;
    
    private bool isHoldingItem = false;
    private WasteContainer currentContainer;

    public override void OnInteract()
    {
        base.OnInteract();

        if (!isHoldingItem)
        {
            wasteItem.PickUp(holdPoint);
            isHoldingItem = true;
        }
        else
        {
            if (currentContainer != null)
            {
                if (currentContainer.CanAcceptItem(wasteItem))
                {
                    wasteItem.Drop();
                    isHoldingItem = false;
                }
                else
                {
                    wasteItem.ShowInvalidPlacement();
                }
            }
            else
            {
                wasteItem.Drop();
                isHoldingItem = false;
            }
        }
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
}