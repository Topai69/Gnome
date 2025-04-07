using UnityEngine;

public class WindResistance : MonoBehaviour
{
    public float windStrength = 225f;
    public Vector3 windDirection = Vector3.forward; 
    public KeyCode sprintKey = KeyCode.LeftShift; 
    public float sprintResistanceMultiplier = 1.5f; 

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDirection = windDirection.normalized;

                
                bool isSprinting = Input.GetKey(sprintKey);

                float finalStrength = isSprinting ? windStrength * sprintResistanceMultiplier : windStrength;

                
                rb.AddForce(forceDirection * finalStrength, ForceMode.Force);
            }
        }
    }
}
