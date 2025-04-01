using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier; 
    public bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space; // key to jump

    [Header("Ground Check")]
    //public float playerHeight;
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask whatIsGround; // for ground check
    bool grounded;

    public Transform orientation;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Vector3 smoothMoveDirection;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        readyToJump = true;
        rb.freezeRotation = true;
    }

    void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround); 
        HandleInput();
        SpeedControl();
        if (grounded)
            rb.drag = groundDrag; 
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
     
        if (Input.GetKey(jumpKey) && readyToJump && grounded) 
        {
            readyToJump = false;
            Jump(); 
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f * airMultiplier, ForceMode.Force);
    }


    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // keep y velocity the same

        if (flatVel.magnitude > movementSpeed) // if the velocity is greater than the movement speed
        {
            // limit the velocity
            Vector3 limitedVel = flatVel.normalized * movementSpeed; // get the limited velocity
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); // apply it to the rb
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // reset y velocity before jump to avoid adding to it
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); // add jump force
    }

    private void ResetJump()
    {
        readyToJump = true; // reset the jump ability
    }
}
