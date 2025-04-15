using UnityEngine;

public class ClimbableRope : MonoBehaviour
{
    [Header("Rope Settings")]
    public float climbSpeed = 3f;
    public KeyCode climbKey = KeyCode.E;

    private PlayerMovement currentPlayer;
    private bool isPlayerOnRope;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayer = other.GetComponent<PlayerMovement>();
            if (currentPlayer != null)
            {
                isPlayerOnRope = true;
                currentPlayer.isOnRope = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isPlayerOnRope && currentPlayer != null && Input.GetKey(climbKey))
        {
            float verticalInput = Input.GetAxisRaw("Vertical");
            
            if (verticalInput != 0)
            {
                currentPlayer.rb.useGravity = false;
                currentPlayer.rb.linearVelocity = new Vector3(0, verticalInput * climbSpeed, 0);
                
                Vector3 newPos = currentPlayer.transform.position;
                newPos.x = transform.position.x;
                newPos.z = transform.position.z;
                currentPlayer.transform.position = newPos;
            }
            else
            {
                currentPlayer.rb.linearVelocity = Vector3.zero;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && currentPlayer != null)
        {
            isPlayerOnRope = false;
            currentPlayer.isOnRope = false;
            currentPlayer.rb.useGravity = true;
            currentPlayer = null;
        }
    }
}