using UnityEngine;

public class BallControl : MonoBehaviour
{
    [Header("Ball Control")]
    [Space]
    [Header("References")]
    [SerializeField] Rigidbody rb;
    [Space]
    [Header("Configuration")]
    [Space]
    [Header("Detection")]
    [SerializeField] float detectionVerticalOffset = 0.75f;
    [SerializeField] float ballDetectionRange = 0.75f;
    [SerializeField] LayerMask ballLayer;
    [Space]
    [Header("States")]
    [SerializeField] public bool canControlBall = true;

    void OnDrawGizmosSelected()
    {
        DebugBallControlRange();
    }

    void DebugBallControlRange()
    {
        Gizmos.color = Color.green;
        Vector3 detectionPosition = transform.position - new Vector3(0, detectionVerticalOffset, 0) + transform.forward;
        Gizmos.DrawSphere(detectionPosition, ballDetectionRange);
    }

    void Start()
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        if (!rb)
        {
            Debug.LogError("One or more references are missing in the BallControl script.", gameObject);
            return;
        }
    }

    void FixedUpdate()
    {
        DetectAndControlBall();
    }

    void DetectAndControlBall()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position - new Vector3(0, detectionVerticalOffset, 0) + transform.forward, ballDetectionRange, ballLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody ballRigidbody = colliders[i].GetComponent<Rigidbody>();
            if (!ballRigidbody || !canControlBall)
            {
                Debug.LogError("Ball Rigidbody not found or you can't controll the ball!", gameObject);
                return;
            }
            ballRigidbody.velocity = rb.velocity;
        }
    }
}
