using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    private Animator Gnome;
    private float movementThreshold = 0.1f;
    private PlayerMovement playerMovement;
    private bool jumpInitiated = false; 
    private float jumpInitiationTimer = 0f;


    void Start()
    {
        Gnome = GetComponent<Animator>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        if (!playerMovement.blockAInput)
        { 
            if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.JoystickButton1)) && playerMovement != null && playerMovement.grounded)
            {
                jumpInitiated = true;
                jumpInitiationTimer = 0.2f;
                Gnome.SetBool("IsJumping", true);
                Gnome.SetBool("IsIdle", false);
                Gnome.SetBool("IsWalking", false);
                Gnome.SetBool("IsFalling", false);
            }

        if (jumpInitiationTimer > 0)
        {
            jumpInitiationTimer -= Time.deltaTime;
        }

        if (playerMovement != null && !playerMovement.grounded)
        {
            if (playerMovement.GetVerticalVelocity() > 0 || jumpInitiated)
            {
                Gnome.SetBool("IsJumping", true);
                Gnome.SetBool("IsFalling", false);

                if (playerMovement.GetVerticalVelocity() > 0)
                {
                    jumpInitiated = false;
                }
            }
            else
            {
                Gnome.SetBool("IsJumping", false);
                Gnome.SetBool("IsIdle", false);
                Gnome.SetBool("IsWalking", false);
                Gnome.SetBool("IsFalling", true);
                jumpInitiated = false;
            }
            Gnome.SetFloat("MovementSpeed", 0f);
            return;
        }
        else
        {
            if (jumpInitiationTimer <= 0)
            {
                jumpInitiated = false;
            }

            if (!jumpInitiated)
            {
                Gnome.SetBool("IsJumping", false);
                Gnome.SetBool("IsFalling", false);
            }
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        bool isMoving = Mathf.Abs(horizontalInput) > movementThreshold || Mathf.Abs(verticalInput) > movementThreshold;

        if (!jumpInitiated)
        {
            Gnome.SetBool("IsIdle", !isMoving);
            Gnome.SetBool("IsWalking", isMoving);
        }

        float movementSpeed = new Vector2(horizontalInput, verticalInput).magnitude;
        Gnome.SetFloat("MovementSpeed", movementSpeed);
        }
    }
}