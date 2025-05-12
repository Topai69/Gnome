using UnityEngine;

public class WasteContainer : MonoBehaviour
{
    [SerializeField] private WasteType acceptedType;
    [SerializeField] private Transform dropPoint;
    
    public WasteType AcceptedType => acceptedType;
    public Transform DropPoint => dropPoint;

    public bool CanAcceptItem(WasteInteraction item)
    {
        return item.Type == acceptedType;
    }
}