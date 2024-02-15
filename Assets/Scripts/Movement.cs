using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [Space]
    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rb;
    [Space]
    [Header("Configuration")]
    [SerializeField] KeyCode jogLeftKey = KeyCode.Q;
    [SerializeField] KeyCode jogRightKey = KeyCode.E;
    [SerializeField] public float speed = 1000f;
    [SerializeField] public float maxSpeed = 5f;
    [SerializeField] float stopSmoothness = 15f;
    [SerializeField] float rotationSmoothness = 5f;
    [SerializeField] float rigidbodyDrag = 0.5f;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public Vector3 lastDirection;
    [Space]
    [Header("States")]
    [SerializeField] public bool canMove = true;

    void OnDrawGizmosSelected()
    {
        DebugDirectionGizmos();
    }

    void DebugDirectionGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 dir = new Vector3(transform.position.x + direction.x * 2f, transform.position.y, transform.position.z + direction.z * 2f);
        Gizmos.DrawLine(transform.position, dir);
    }

    void Start()
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        if (!animator || !rb)
        {
            Debug.LogError("One or more references are missing in the Movement script.", gameObject);
            return;
        }
        rb.drag = rigidbodyDrag;
    }

    void Update()
    {
        CalculateDirection();
    }

    void FixedUpdate()
    {
        Move();
        RotateTowardDirection();
        HandleJogging();
        ClampPlayerVelocity();
    }

    void CalculateDirection()
    {
        float directionX = Input.GetAxisRaw("Horizontal");
        float directionZ = Input.GetAxisRaw("Vertical");
        direction = new Vector3(directionX, 0f, directionZ).normalized;
    }

    void Move()
    {
        if (direction == Vector3.zero || !canMove)
        {
            rb.velocity = Vector3.Slerp(rb.velocity, new Vector3(0, rb.velocity.y, 0), Time.fixedDeltaTime * stopSmoothness);
            return;
        }
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.Force);
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

    void HandleJogging()
    {
        if (Input.GetKeyDown(jogLeftKey))
        {
            rb.AddForce(-transform.right * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(jogRightKey))
        {
            rb.AddForce(transform.right * 5f, ForceMode.Impulse);
        }
    }
    void ClampPlayerVelocity()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }
}
