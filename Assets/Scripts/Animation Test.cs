using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    private Animator Gnome;
    private float movementThreshold = 0.1f; 
    private bool lastMovementState = false;
    
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer; 
    private bool isGrounded = true;
    private Rigidbody rb;
    
    void Start()
    {
        Gnome = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
 
        bool isMoving = Mathf.Abs(horizontalInput) > movementThreshold || 
                        Mathf.Abs(verticalInput) > movementThreshold;
     
        CheckGrounded();
        
        if (!isGrounded && rb.linearVelocity.y < -0.1f) 
        {
            Gnome.SetBool("IsFalling", true);
            Gnome.SetBool("IsIdle", false);
            Gnome.SetBool("IsWalking", false);
        }
        else 
        {
            Gnome.SetBool("IsFalling", false);
            Gnome.SetBool("IsIdle", !isMoving);
            Gnome.SetBool("IsWalking", isMoving);
        }
        
        float movementSpeed = new Vector2(horizontalInput, verticalInput).magnitude;
        Gnome.SetFloat("MovementSpeed", movementSpeed);
    }
    
    void CheckGrounded()
    {
        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.1f; 
        
        isGrounded = Physics.Raycast(rayStart, Vector3.down, out hit, groundCheckDistance, groundLayer);
        
        Debug.DrawRay(rayStart, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }
}
