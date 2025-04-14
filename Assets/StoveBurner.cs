using UnityEngine;

public class StoveBurner : MonoBehaviour
{
    public float heatTime = 3f;
    public float launchForce = 20f;
    public string playerTag = "Player";

    private bool playerOnBurner = false;
    private float heatTimer = 0f;
    private PlayerMovement playerMovement;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerOnBurner = true;
            playerMovement = other.GetComponent<PlayerMovement>();
            heatTimer = 0f;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (playerOnBurner && other.CompareTag(playerTag))
        {
            if (playerMovement != null && playerMovement.grounded)
            {
                heatTimer += Time.deltaTime;

                if (heatTimer >= heatTime)
                {
                    Rigidbody rb = other.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reset Y velocity
                        rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
                    }

                    heatTimer = 0f;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerOnBurner = false;
            heatTimer = 0f;
            playerMovement = null;
        }
    }
}
