using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [Header("Ground Detection")]

    [Header("References")]

    [Header("Objects")]
    [SerializeField] Transform groundSensor; // Sensor used for ground detection

    [Header("Configuration")]
    [SerializeField, Range(0.1f, 0.5f)] float detectionRange = 0.1f; // Range for ground detection
    [SerializeField] LayerMask groundLayer; // Layer mask for ground detection

    // Function to provide feedback about ground detection
    public GroundFeedback GetGroundFeedback()
    {
        // Perform a raycast downward from the ground sensor position to detect ground
        bool isGrounded = Physics.Raycast(groundSensor.position, -groundSensor.up, out RaycastHit groundHit, detectionRange, groundLayer);

        // Return the result of ground detection
        return new GroundFeedback(isGrounded, groundHit);
    }
}

// Class to hold feedback about ground detection
public class GroundFeedback
{
    public bool isGrounded; // Flag indicating whether the object is grounded
    public RaycastHit hitInfo; // Information about the ground hit (if any)

    // Constructor to initialize ground feedback
    public GroundFeedback(bool isGrounded, RaycastHit hitInfo)
    {
        this.isGrounded = isGrounded; // Assign ground detection result
        this.hitInfo = hitInfo; // Assign ground hit information
    }
}
