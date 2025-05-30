using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    private Animator Gnome;
    private float movementThreshold = 0.1f;

    private PlayerMovement playerMovement;

    void Start()
    {
        Gnome = GetComponent<Animator>();
        playerMovement = GetComponentInParent<PlayerMovement>(); // Get reference from parent
    }

    void Update()
    {
        if (playerMovement != null && !playerMovement.grounded)
        {
            Gnome.SetBool("IsIdle", false);
            Gnome.SetBool("IsWalking", false);
            Gnome.SetBool("IsFalling", true);
            Gnome.SetFloat("MovementSpeed", 0f);
            return;
        }
        else
        {
            Gnome.SetBool("IsFalling", false);
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        bool isMoving = Mathf.Abs(horizontalInput) > movementThreshold || Mathf.Abs(verticalInput) > movementThreshold;

        Gnome.SetBool("IsIdle", !isMoving);
        Gnome.SetBool("IsWalking", isMoving);

        float movementSpeed = new Vector2(horizontalInput, verticalInput).magnitude;
        Gnome.SetFloat("MovementSpeed", movementSpeed);
    }
}