using UnityEngine;

public class CameraLock : MonoBehaviour
{
    [Header("Camera Lock")]
    [Space]
    [Header("References")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform ballTarget;
    [SerializeField] public Transform playerTarget;
    [Space]
    [Header("Configuration")]
    [SerializeField] float followSpeed = 0.5f;
    [HideInInspector] float initialPositionZ;

    void Start()
    {
        InitializeComponents();
        initialPositionZ = transform.position.z;
    }

    void InitializeComponents()
    {
        if (!mainCamera || !ballTarget || !playerTarget)
        {
            Debug.LogError("One or more references are missing in the CameraLock script.", gameObject);
            return;
        }
    }

    void Update()
    {
        MoveCameraToTargetPosition();
        RotateCameraTowardsTargets();
    }

    void RotateCameraTowardsTargets()
    {
        Quaternion lookRotationBall = Quaternion.LookRotation(ballTarget.position - transform.position);
        Quaternion lookRotationPlayer = Quaternion.LookRotation(playerTarget.position - transform.position);
        Quaternion crossRotation = Quaternion.Slerp(lookRotationPlayer, lookRotationBall, 0.5f);

        transform.rotation = Quaternion.Lerp(transform.rotation, crossRotation, followSpeed * Time.deltaTime);
    }

    void MoveCameraToTargetPosition()
    {
        float distanceToBall = Vector3.Distance(playerTarget.transform.position, ballTarget.position);
        float targetPositionX = playerTarget.position.x + (distanceToBall * 0.5f);

        Vector3 targetPosition = new Vector3(targetPositionX, transform.position.y, initialPositionZ);

        transform.position = Vector3.Slerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
