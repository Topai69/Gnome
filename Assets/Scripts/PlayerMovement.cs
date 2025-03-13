using System.Collections;
using System.Collections.Generic;
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
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded == true)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
        Debug.Log(stamina);
        if (stamina >= 100)
        {
            stamina = 100;
        }
        else if (stamina <= 1)
        {
            StartCoroutine(Tired());
        }

        staminaText.text = stamina.ToString();
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
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            moveSpeed = 10f;
            StartCoroutine(StaminaLevelDecrease());
        }
        else
        {
            moveSpeed = 5f;
            StartCoroutine(StaminaLevelIncrease());
        }
        // player is on ground
        if (grounded == true)
            //rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            rb.linearVelocity = new Vector3(moveDirection.normalized.x * moveSpeed, rb.linearVelocity.y, moveDirection.normalized.z * moveSpeed);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
    IEnumerator StaminaLevelDecrease()
    {
        stamina -= 10f * Time.deltaTime;
        yield return new WaitForSeconds(1);
    }
    IEnumerator StaminaLevelIncrease()
    {
        stamina += 10f * Time.deltaTime;
        yield return new WaitForSeconds(1);
    }
    IEnumerator Tired()
    {
        new WaitForSeconds(2f);
        StartCoroutine(StaminaLevelIncrease());
        yield break;
    }
}