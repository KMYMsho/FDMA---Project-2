using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveForceMultiplier = 40; // Force applied is direction * this value
    [SerializeField] private float maxSpeed = 10;          // Maximum horizontal velocity
    [SerializeField] private float groundDrag = 5f;          // Rigidbody drag when grounded
    [SerializeField] private float airDrag = 1.5f;           // Rigidbody drag when airborne (small value recommended)
    [SerializeField][Range(0f, 1f)] private float airMultiplier = 0.3f; // How much force multiplier applies in air (0=none, 1=full)

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 7f;         // Upward impulse force for jumping
    [SerializeField] private float jumpCooldown = 0.25f;      // Time in seconds between allowed jumps
    private bool readyToJump = true;
    private float jumpCooldownTimer = 0f;

    [Header("Sprint")]
    [SerializeField] private float sprintMultiplier = 1.5f; // Multiplier for sprinting speed

    [Header("Keybinds")] // Consider using Input Manager names for flexibility
    [SerializeField] private KeyCode jumpKey = KeyCode.Space; // Or: [SerializeField] private string jumpButton = "Jump";
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift; // Key to activate sprinting

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;         // Point at the base of the player
    [SerializeField] private float groundDistance = 0.5f;   // Radius of the CheckSphere
    [SerializeField] private LayerMask whatIsGround;        // Layers considered ground
    private bool isGrounded;                             // Flag if player is grounded

    [Header("References")]
    [SerializeField] private Transform orientation;          // Determines forward/right direction for movement
    private Rigidbody rb;
    private Vector2 inputVector;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent physics rotation

        if (orientation == null)
        {
            Debug.LogWarning("PlayerMovement: Orientation Transform not assigned. Movement direction might be incorrect.", this);
            orientation = transform; // Fallback to player's own transform if none provided
        }
        if (groundCheck == null)
        {
            Debug.LogError("PlayerMovement: Ground Check Transform not assigned. Ground detection will fail.", this);
        }
    }

    void Update()
    {
        // --- Ground Check ---
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround, QueryTriggerInteraction.Ignore);

        // --- Handle Input ---
        HandleInput();

        // --- Handle Drag ---
        rb.drag = isGrounded ? groundDrag : airDrag;

        // --- Handle Jump Cooldown ---
        if (!readyToJump)
        {
            jumpCooldownTimer -= Time.deltaTime;
            if (jumpCooldownTimer <= 0)
            {
                ResetJump();
            }
        }

        // --- Speed Control ---
        SpeedControl();
    }

    void FixedUpdate()
    {
        // --- Apply Movement Force ---
        MovePlayer();
    }

    private void HandleInput()
    {
        inputVector.x = Input.GetAxisRaw("Horizontal"); // Use GetAxisRaw for immediate response
        inputVector.y = Input.GetAxisRaw("Vertical");

        // --- Jump Input ---
        // Use GetKeyDown for single press detection
        // Or Input.GetButtonDown(jumpButton) if using Input Manager name
        if (Input.GetKeyDown(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;
            jumpCooldownTimer = jumpCooldown; // Start cooldown timer
            Jump();

            // Optional: Slight delay before allowing next jump check if needed, but cooldown timer handles this
            // Invoke(nameof(ResetJump), jumpCooldown); // Avoid Invoke - using timer instead
        }
    }

    private void MovePlayer()
    {
        // Calculate movement direction based on orientation
        Vector3 moveDirection = orientation.forward * inputVector.y + orientation.right * inputVector.x;
        moveDirection.Normalize(); // Prevent faster diagonal movement

        // Apply force differently based on ground status
        float currentForceMultiplier = moveForceMultiplier;
        if (!isGrounded)
        {
            currentForceMultiplier *= airMultiplier;
        }

        if (Input.GetKey(sprintKey) && isGrounded)
        {
            currentForceMultiplier *= sprintMultiplier; // Apply sprint multiplier
        }

        // Apply force to move the player
        // We multiply by 10 potentially as a carry-over, let's integrate it into moveForceMultiplier
        // If it still feels weak, increase moveForceMultiplier directly.
        rb.AddForce(moveDirection * currentForceMultiplier, ForceMode.Force);

        // Note: ForceMode.Force applies force over time, considering mass and delta time implicitly.
        // Adjust moveForceMultiplier considering your Rigidbody's mass. Higher mass needs more force.
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocity if needed
        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); // Apply the limited velocity
        }
    }

    private void Jump()
    {
        // Reset y velocity before jumping ensures consistent jump height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Add the jump force
        // ForceMode.Impulse applies an instantaneous force, factoring in mass. Good for jumps.
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    // Optional: Visualize ground check sphere in Scene view for debugging
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}