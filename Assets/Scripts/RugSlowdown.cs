using UnityEngine;

public class RugSlowdown : MonoBehaviour
{
    public float slowSpeed = 2f; // Rug slow speed

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            PlayerStamina playerStamina = other.GetComponent<PlayerStamina>();

            if (playerMovement && playerStamina)
            {
                playerMovement.overrideSpeed = true;
                playerMovement.customSpeed = slowSpeed;

                playerStamina.sprintAllowed = false; // Disable sprint
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            PlayerStamina playerStamina = other.GetComponent<PlayerStamina>();

            if (playerMovement && playerStamina)
            {
                playerMovement.overrideSpeed = false;
                playerStamina.sprintAllowed = true; // Re-enable sprint
            }
        }
    }
}
