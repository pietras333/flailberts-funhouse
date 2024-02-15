using UnityEngine;

public class BallData : MonoBehaviour
{
    [Header("Ball Data")]
    [Space]
    [Header("References")]
    [SerializeField] Rigidbody ballRb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [Space]
    [Header("Configuration")]
    [SerializeField] float groundCheckRange = 0.25f;
    [SerializeField] float groundedMaxSpeed = 13f;
    [SerializeField] float inAirMaxSpeed = 15f;
    [SerializeField] string playerTag = "Player";
    [SerializeField] float ballMaxSpeed;
    [Space]
    [Header("States")]
    [SerializeField] bool isGrounded;
    [HideInInspector] GameObject lastTouchedBy;

    void Start()
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        if (!ballRb || !groundCheck)
        {
            Debug.LogError("One or more references are missing in the BallData script.", gameObject);
            return;
        }

    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRange, groundLayer);
        ballMaxSpeed = isGrounded ? groundedMaxSpeed : inAirMaxSpeed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
    }

    void HandleCollision(Collision collision)
    {
        GameObject colliderGameObject = collision.gameObject;
        if (colliderGameObject != lastTouchedBy && colliderGameObject.CompareTag(playerTag))
        {
            lastTouchedBy = colliderGameObject;
        }
    }

    void FixedUpdate()
    {
        ClampBallVelocity();
    }

    void ClampBallVelocity()
    {
        ballRb.velocity = Vector3.ClampMagnitude(ballRb.velocity, ballMaxSpeed);
    }
}
