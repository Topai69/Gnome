using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    private Animator Gnome;
    public PlayerMovement pr;
    private float movementThreshold = 0.1f;
    

    void Start()
    {
        Gnome = GetComponent<Animator>();
        pr = GetComponent<PlayerMovement>();
    }

    public void Update()
    {
        
     
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            bool isMoving = (Mathf.Abs(horizontalInput) > movementThreshold) ||
                            (Mathf.Abs(verticalInput) > movementThreshold);

            Gnome.SetBool("IsIdle", !isMoving);

                Gnome.SetBool("IsWalking", isMoving);

                float movementSpeed = new Vector2(horizontalInput, verticalInput).magnitude;
                Gnome.SetFloat("MovementSpeed", movementSpeed);
      
    }
}


