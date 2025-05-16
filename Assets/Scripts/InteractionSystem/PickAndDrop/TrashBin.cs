using UnityEngine;

public class TrashBin : MonoBehaviour
{
    [SerializeField] private TrashType acceptedType;
    [SerializeField] private PopUpManager popUpManager;
    
    private void Start()
    {
        if (popUpManager == null)
        {
            popUpManager = FindObjectOfType<PopUpManager>();
        }
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
                Destroy(item.gameObject);
            }
        }
    }
    
    private void ShowWrongBinPopup(ItemGrabbable item)
    {
        Vector3 popupPosition = transform.position + Vector3.up * 1.5f;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(popupPosition);
        screenPos.z = -Camera.main.transform.position.z;
        
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        
        GameObject popUpObject = Instantiate(popUpManager.GetPopUpPrefab(), worldPos, Quaternion.identity);
        popUpObject.GetComponent<TrashBinPopup>().text_value = "WRONG BIN";
        
        if (item.TryGetComponent(out Rigidbody rb))
        {
            Vector3 pushDirection = (item.transform.position - transform.position).normalized;
            rb.AddForce(pushDirection * 5f, ForceMode.Impulse);
        }
    }
}