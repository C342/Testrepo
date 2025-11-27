using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 20f;
    public float deceleration = 25f;

    [Header("Jump")]
    public float jumpForce = 8f;
    public float variableJumpMultiplier = 0.5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private float moveInput;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        CheckGround();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, 0);
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * variableJumpMultiplier, 0);
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = moveInput * moveSpeed;
        float speedDif = targetSpeed - rb.linearVelocity.x;

        float accel = (Mathf.Abs(moveInput) > 0.01f) ? acceleration : deceleration;
        float movement = speedDif * accel * Time.fixedDeltaTime;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x + movement, rb.linearVelocity.y, 0);
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, groundLayer);
    }
}