using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float stamina = 100.0f;
    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public TextMeshProUGUI staminaText;

    private PlayerStamina playerStamina; 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerStamina = GetComponent<PlayerStamina>(); 
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
        {
            rb.linearDamping = groundDrag; 
        }
        else
        {
            rb.linearDamping = 0;
        }

        // Removed previous stamina logic bcs it's handled in PlayerStamina now (entirely)

        staminaText.text = Mathf.RoundToInt(playerStamina.currentStamina).ToString(); 
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // calculates movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        float targetSpeed = moveSpeed;

        
        if (Input.GetKey(playerStamina.sprintKey) && playerStamina.currentStamina > 0f && playerStamina.sprintAllowed)
        {
            targetSpeed = 10f;
        }
        else
        {
            targetSpeed = 5f;
        }

        // player is on ground
        if (grounded)
            rb.linearVelocity = new Vector3(moveDirection.normalized.x * targetSpeed, rb.linearVelocity.y, moveDirection.normalized.z * targetSpeed);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        float maxSpeed = (Input.GetKey(playerStamina.sprintKey) && playerStamina.currentStamina > 0f && playerStamina.sprintAllowed) ? 10f : moveSpeed;

        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    // Removed stamina coroutines aka StaminaLevelDecrease, StaminaLevelIncrease and Tired bcs of conflicts
}
