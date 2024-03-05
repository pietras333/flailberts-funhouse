using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [Space]
    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rb;
    [SerializeField] RunHandler runHandler;
    [SerializeField] Transform orientation;
    [SerializeField] Transform groundCheck;
    [Space]
    [Header("Configuration")]
    [Space]
    [Header("Basic locomotion")]
    [HideInInspector] public Vector3 lastDirection;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] float initialSpeed;
    [HideInInspector] public float initialMaxSpeed;
    [SerializeField] public float speed = 1000f;
    [SerializeField] public float maxSpeed = 5f;
    [Space]
    [Header("Smothness")]
    [SerializeField] float stopSmoothness = 15f;
    [SerializeField] float rotationSmoothness = 5f;
    [SerializeField] float rigidbodyDrag = 0.5f;
    [Space]
    [Header("Ground detecion")]
    [SerializeField] float groundDetectionRange = 0.1f;
    [SerializeField] LayerMask groundLayer;
    [Space]
    [Header("Dribbling")]
    [SerializeField] public float dribbleSpeed;
    [SerializeField] float dribbleSpeedMultiplier = 0.85f;
    [SerializeField] public float dribbleMaxSpeed;
    [SerializeField] float dribbleMaxSpeedMultiplier = 0.8f;
    [Header("Jumping")]
    [Space]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] float jumpForce = 20f;
    [SerializeField] float jumpCooldown = 0.1f;
    [Space]
    [Header("Gravity")]
    [SerializeField] float gravity = 9f;
    [SerializeField] float airMaxSpeed = 15f;
    [Space]
    [Header("Slope Movement")]
    [SerializeField] float maxSlopeAngle = 45f;
    [HideInInspector] RaycastHit slopeHit;
    [Space]
    [Header("States")]
    [SerializeField] public bool canMove = true;
    [SerializeField] public bool isJumping;
    [SerializeField] bool isGrounded;

    void OnDrawGizmos()
    {
        DebugDirectionGizmos();
    }

    void DebugDirectionGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 dir = new Vector3(transform.position.x + direction.x * 2f, transform.position.y, transform.position.z + direction.z * 2f);
        if (!IsOnSlope())
        {
            Gizmos.DrawLine(transform.position, dir);
            return;
        }
        Gizmos.color = Color.magenta;
        Vector3 slopeDirection = GetSlopeMoveDirection();
        Vector3 sDir = dir + slopeDirection;
        Gizmos.DrawLine(transform.position, sDir);
    }

    void Start()
    {
        InitializeComponents();
        initialSpeed = speed;
        dribbleSpeed = initialSpeed * dribbleSpeedMultiplier;
        initialMaxSpeed = maxSpeed;
        dribbleMaxSpeed = initialMaxSpeed * dribbleMaxSpeedMultiplier;
    }

    void InitializeComponents()
    {
        if (!animator || !rb || !runHandler || !transform)
        {
            Debug.LogError("One or more references are missing in the Movement script.", gameObject);
            return;
        }
        rb.drag = rigidbodyDrag;
    }

    void Update()
    {
        CalculateDirection();
        HandleMovementValues();
    }

    void FixedUpdate()
    {
        Move();
        Jump();
        RotateTowardDirection();
        ClampPlayerVelocity();
        HandleGravity();
        HandleMomentum();
    }

    void HandleMovementValues()
    {
        isGrounded = Physics.Raycast(groundCheck.position, -transform.up, groundDetectionRange, groundLayer);
        isJumping = Input.GetKey(jumpKey);

        if (runHandler.isRunning)
        {
            return;
        }

        maxSpeed = !isGrounded ? airMaxSpeed : initialMaxSpeed;
        speed = initialSpeed;
        // rb.useGravity = !IsOnSlope();

        animator.SetBool("isRunning", direction != Vector3.zero);
        animator.SetBool("isGrounded", isGrounded);
    }

    void CalculateDirection()
    {
        float directionX = Input.GetAxisRaw("Horizontal");
        float directionZ = Input.GetAxisRaw("Vertical");
        direction = directionX * orientation.right + directionZ * orientation.forward;
    }

    void Move()
    {
        if (IsOnSlope())
        {
            Vector3 slopeDirection = GetSlopeMoveDirection();
            rb.AddForce(speed * Time.fixedDeltaTime * slopeDirection, ForceMode.Force);
            rb.AddForce(80f * -orientation.up * Time.fixedDeltaTime);
        }
        else
        {
            rb.AddForce(speed * Time.fixedDeltaTime * direction, ForceMode.Force);
        }
    }

    void HandleMomentum()
    {
        if (IsOnSlope())
        {
            if (rb.velocity.magnitude > maxSpeed)
                rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        else
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            // limit velocity if needed
            if (flatVelocity.magnitude > maxSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }

    void RotateTowardDirection()
    {
        if (direction != Vector3.zero)
        {
            lastDirection = direction;
        }
        Quaternion targetRotation = Quaternion.LookRotation(lastDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothness * Time.deltaTime);
    }

    void ClampPlayerVelocity()
    {
        // rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    void HandleGravity()
    {
        if (!isGrounded && !IsOnSlope())
        {
            rb.AddForce(-transform.up * gravity, ForceMode.Acceleration);
        }
    }

    void Jump()
    {
        if (isJumping && isGrounded)
        {
            rb.AddForce(orientation.up * jumpForce, ForceMode.Force);
        }
    }

    bool IsOnSlope()
    {
        if (Physics.Raycast(groundCheck.position, Vector3.down, out slopeHit, groundDetectionRange * 2f, groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle <= maxSlopeAngle && angle != 0;
        }
        return false;
    }

    Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}