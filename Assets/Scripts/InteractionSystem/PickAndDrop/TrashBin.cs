using UnityEngine;
using System.Collections.Generic;

public class TrashBin : MonoBehaviour
{
    [SerializeField] private TrashType acceptedType;
    [SerializeField] private TaskManager taskManager;
    [HideInInspector] public ScoreScript scoreScript;

    private static HashSet<int> correctlySortedInstanceIDs = new HashSet<int>();
    private static int requiredCorrectTrash = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ItemGrabbable item))
        {
            int instanceID = item.GetInstanceID();

            if (item.GetTrashType() != acceptedType)
            {
                if (!item.HasShownWrongBinPopup())
                {
                    ShowWrongBinPopup(item);
                    item.SetWrongBinPopupShown(true);
                }
            }
            else if (!correctlySortedInstanceIDs.Contains(instanceID))
            {
                correctlySortedInstanceIDs.Add(instanceID);

                if (scoreScript != null)
                    scoreScript.Score += 20;

                Debug.Log($"Correctly sorted: {correctlySortedInstanceIDs.Count}");

                if (correctlySortedInstanceIDs.Count >= requiredCorrectTrash && taskManager != null)
                {
                    taskManager.CompleteTask(5);
                    Debug.Log("Task6 Completed!");
                }

                Destroy(item.gameObject, 1.5f);
            }
        }
    }

    private void ShowWrongBinPopup(ItemGrabbable item)
    {
        if (item.TryGetComponent(out Rigidbody rb))
        {
            Vector3 pushDirection = (item.transform.position - transform.position).normalized;
            rb.AddForce(pushDirection * 5f, ForceMode.Impulse);
        }
    }
}