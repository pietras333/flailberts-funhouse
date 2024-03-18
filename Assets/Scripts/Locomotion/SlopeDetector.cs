using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlopeDetector : MonoBehaviour
{
    [Header("Slope Handler")]
    [Space]
    [Header("References")]
    [Header("Scripts")]
    [SerializeField] InputReceiver inputReceiver; // Reference to the input receiver script
    [SerializeField] GroundDetector groundDetector; // Reference to the ground detector script

    [Header("Configuration")]
    [SerializeField, Range(35f, 100f)] float maxSlopeAngle = 45f; // Maximum slope angle considered as walkable

    // Function to provide feedback about slope detection
    public SlopeDetectionFeedback GetSlopeDetectionFeedback()
    {
        // Check if the object is grounded
        if (groundDetector.GetGroundFeedback().isGrounded)
        {
            // Calculate the angle between the ground normal and the up vector
            float angle = Vector3.Angle(Vector3.up, groundDetector.GetGroundFeedback().hitInfo.normal);

            // Check if the angle is within the acceptable slope range and not flat (angle != 0)
            bool checkForSlope = angle <= maxSlopeAngle && angle != 0;

            // If the angle indicates a slope, calculate the direction of movement on the slope
            if (checkForSlope)
            {
                // Project the input direction onto the slope's plane and normalize it
                Vector3 slopeMoveDirection = Vector3.ProjectOnPlane(inputReceiver.GetInputFeedback().direction, groundDetector.GetGroundFeedback().hitInfo.normal).normalized;

                // Return slope detection feedback
                return new SlopeDetectionFeedback(checkForSlope, slopeMoveDirection);
            }
        }

        // If not on a slope, return feedback indicating no slope
        return new SlopeDetectionFeedback(false, Vector3.zero);
    }
}

// Class to hold feedback about slope detection
public class SlopeDetectionFeedback
{
    public bool isOnSlope; // Flag indicating whether the object is on a slope
    public Vector3 slopeDirection; // Direction of movement on the slope

    // Constructor to initialize slope detection feedback
    public SlopeDetectionFeedback(bool isOnSlope, Vector3 slopeDirection)
    {
        this.isOnSlope = isOnSlope; // Assign slope detection result
        this.slopeDirection = slopeDirection; // Assign slope movement direction
    }
}
