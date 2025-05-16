using UnityEngine;

public class TrashBin : MonoBehaviour
{
    [SerializeField] private TrashType acceptedType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ItemGrabbable item))
        {
            if (item.GetTrashType() == acceptedType)
            {
                Debug.Log("YES");
                Destroy(item.gameObject);
            }
            else
            {
                Debug.Log("NO");
            }
        }
    }
}
