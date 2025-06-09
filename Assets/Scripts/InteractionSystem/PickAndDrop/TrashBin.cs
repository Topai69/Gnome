using UnityEngine;

public class TrashBin : MonoBehaviour
{
    [SerializeField] private TrashType acceptedType;
    
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ItemGrabbable item))
        {
            if (item.GetTrashType() != acceptedType)
            {
                if (!item.HasShownWrongBinPopup())
                {
                    ShowWrongBinPopup(item);
                    item.SetWrongBinPopupShown(true);
                }
            }
            else
            {
                Destroy(item.gameObject, 1.5f);
            }
        }
    }
    
    private void ShowWrongBinPopup(ItemGrabbable item)
    {
        Vector3 popupPosition = transform.position + Vector3.up * 1.5f;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(popupPosition);
        screenPos.z = -Camera.main.transform.position.z;
        
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        
        if (item.TryGetComponent(out Rigidbody rb))
        {
            Vector3 pushDirection = (item.transform.position - transform.position).normalized;
            rb.AddForce(pushDirection * 5f, ForceMode.Impulse);
        }
    }
}