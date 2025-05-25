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

    [Header("Model Rotation")]
    public Transform orientation; 
    public Transform playerModel; 
    public float turnSmoothTime = 0.1f;

    [Header("Camera Reference")]
    public Transform cameraHolder;        

    float turnSmoothVelocity;
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
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(grappleKey)) StartGrapple();

        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround)
            || Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsInteractable);

        // Sync orientation with camera horizontal direction
        if (cameraHolder != null && orientation != null)
        {
            Vector3 viewDir = cameraHolder.forward;
            viewDir.y = 0f;
            orientation.forward = viewDir.normalized;
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

        // Rotate model if there's movement
        if (playerModel != null && moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(playerModel.localEulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            Quaternion fixedRotation = Quaternion.Euler(-90f, smoothAngle, 0f); 
            playerModel.localRotation = fixedRotation;
        }

        // Removed previous stamina logic bcs it's handled in PlayerStamina now (entirely)
        //staminaText.text = Mathf.RoundToInt(playerStamina.currentStamina).ToString(); 
    }

    /*private void OnTriggerStay(Collider other)
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
    }*/

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

        // calculates movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.y = 0;
        moveDirection.Normalize();
    }

    private void MovePlayer()
    {
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
            rb.linearVelocity = new Vector3(moveDirection.x * targetSpeed, rb.linearVelocity.y, moveDirection.z * targetSpeed);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        float maxSpeed = overrideSpeed ? customSpeed :
            (Input.GetKey(playerStamina.sprintKey) && playerStamina.currentStamina > 0f && playerStamina.sprintAllowed) ? 10f : moveSpeed;

        // limit velocity if needed
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

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        grappling = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    private Vector3 velocityToSet;
    private bool enableMovementOnNextTouch;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.linearVelocity = velocityToSet;

    }

    public void ResetRestrictions()
    {
        grappling = false;
        
    }

    [Header("References")]
    private PlayerMovement pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    private bool grappling;


    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    private void LateUpdate()
    {
        // if (grappling)
        //    lr.SetPosition(0, gunTip.position);
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        grappling = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        //lr.enabled = true;
        //lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }




    public void StopGrapple()
    {

        grappling = false;

        grapplingCdTimer = grapplingCd;

        //lr.enabled = false;
    }

    public bool IsGrappling()
    {
        return grappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
   