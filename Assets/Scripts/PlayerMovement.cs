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
    public LayerMask whatIsInteractable;
    public bool grounded;
    
    [Header("Jump & Glide")]
    public float jumpForce = 6f;
    public KeyCode jumpKey = KeyCode.Space; // Jump key
    public float airMultiplier = 1.5f;
    public bool hasJumped;
    public float glideFallSpeed = 2f; // Glide fall speed

    [Header("Rope Climbing")]
    public bool isOnRope = false;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public Rigidbody rb;

    public TextMeshProUGUI staminaText;

    private PlayerStamina playerStamina;

    // external speed override (for rug slowdown)
    public bool overrideSpeed = false;
    public float customSpeed = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStamina = GetComponent<PlayerStamina>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        // ground check
        if (!isOnRope)
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround) ||
            Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsInteractable);
        }

        MyInput();
        
        if (!isOnRope)
        {
            SpeedControl();
        }

        // Jump logic
        if (Input.GetKeyDown(jumpKey) && grounded && !isOnRope)
        {
            Jump();
        }

        // Glide logic
        if (!grounded && !isOnRope && Input.GetKey(jumpKey) && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -glideFallSpeed, rb.linearVelocity.z);
        }

        // handle drag
        if (grounded && !isOnRope)
        {
            rb.linearDamping = groundDrag; 
        }
        else if (!isOnRope)
        {
            rb.linearDamping = 0; 
        }

        // Removed previous stamina logic bcs it's handled in PlayerStamina now (entirely)
        staminaText.text = Mathf.RoundToInt(playerStamina.currentStamina).ToString(); 
    }

    private void OnTriggerStay(Collider other)
    {
        if (true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("mounting");
                StartCoroutine(ClimbSlab(other.transform));
            }
        }
    }

    //this is a coroutine and it handles the incremental movment
    private IEnumerator ClimbSlab(Transform slabTransform)
    {
        float targetY = slabTransform.position.y + 1.5f;
        Vector3 targetPosition = new Vector3(slabTransform.position.x, targetY, transform.position.z);
        Vector3 startPosition = transform.position;
        float climbProgress = 0f;
        float climbSpeed = 0.5f;

        while (climbProgress < 1f)
        {
            climbProgress += Time.deltaTime * climbSpeed;
            climbProgress = Mathf.Clamp01(climbProgress);

            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, climbProgress);
            rb.MovePosition(newPosition);

            yield return null;
        }
    }

    private void FixedUpdate()
    {
        if (!isOnRope)
        {
            MovePlayer();
        }
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

        float targetSpeed;

        // override speed logic
        if (overrideSpeed)
        {
            targetSpeed = customSpeed;
        }
        else if (Input.GetKey(playerStamina.sprintKey) && playerStamina.currentStamina > 0f && playerStamina.sprintAllowed)
        {
            targetSpeed = 10f;
        }
        else
        {
            targetSpeed = moveSpeed;
        }

        // player is on ground
        if (grounded || hasJumped)
        {
            rb.linearVelocity = new Vector3(moveDirection.normalized.x * targetSpeed, rb.linearVelocity.y, moveDirection.normalized.z * targetSpeed);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        
        float maxSpeed;
        if (overrideSpeed)
        {
            maxSpeed = customSpeed;
        }
        else
        {
            maxSpeed = (Input.GetKey(playerStamina.sprintKey) && playerStamina.currentStamina > 0f && playerStamina.sprintAllowed) ? 10f : moveSpeed;
        }

        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // resets vertical velocity before applying jump force
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce * airMultiplier, ForceMode.Impulse);
        hasJumped = true;
    }

    // Removed stamina coroutines aka StaminaLevelDecrease, StaminaLevelIncrease and Tired bcs of conflicts
}
