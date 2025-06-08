using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    [Header("Light Control")]
    public Light targetLight;

    private BoxCollider triggerCollider;

    private void Start()
    {
        triggerCollider = GetComponent<BoxCollider>();

        if (triggerCollider != null)
            triggerCollider.isTrigger = true;

        if (targetLight != null)
            targetLight.enabled = false; // Light starts off
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && targetLight != null)
        {
            targetLight.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && targetLight != null)
        {
            targetLight.enabled = false;
        }
    }
}
