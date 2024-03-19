using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideHandler : MonoBehaviour
{
    [Header("Slide Handler")]
    [Space]
    [Header("References")]
    [SerializeField] Rigidbody playerRigidbody; // Reference to the player's Rigidbody component
    [SerializeField] CapsuleCollider playerCollider; // Reference to the player's CapsuleCollider component
    [Header("Scripts")]
    [SerializeField] InputReceiver inputReceiver; // Script handling player input
    [SerializeField] LocomotionParameters locomotionParameters; // Scriptable object holding locomotion parameters
    [SerializeField] Locomotion locomotion; // Script handling locomotion
    [SerializeField] SlopeDetector slopeDetector; // Script detecting if the player is on a slope
    [HideInInspector] public bool isSliding; // Flag indicating if the player is sliding
    [HideInInspector] float slideTimer; // Timer for the slide duration
    [HideInInspector] Vector3 slideDirection; // Direction of the slide

    void Start()
    {
        // Initialize slide timer
        slideTimer = locomotionParameters.GetSlideParameters().slideDuration;
    }

    void Update()
    {
        // Handle sliding logic
        HandleSliding();
    }

    // Function to handle sliding logic
    void HandleSliding()
    {
        // Start sliding when slide key is pressed and conditions are met
        if (Input.GetKeyDown(inputReceiver.GetInputParametersFeedback().slideKey) && !isSliding && slideTimer > 0)
        {
            StartSlide();
        }

        // Continue sliding while sliding
        if (isSliding)
        {
            HandleSlideMovement();
        }

        // Stop sliding when slide key is released or slide duration expires
        if (isSliding && (slideTimer <= 0 || Input.GetKeyUp(inputReceiver.GetInputParametersFeedback().slideKey)))
        {
            StopSlide();
        }
    }

    // Function to start sliding
    void StartSlide()
    {
        // Adjust player collider height for sliding
        playerCollider.height = 1;
        // Apply initial force to start sliding
        playerRigidbody.AddForce(10f * -transform.up);
        // Determine slide direction based on current movement direction
        slideDirection = locomotion.GetCurrentDirection();
        // Set sliding flag to true
        isSliding = true;
    }

    // Function to handle slide movement
    void HandleSlideMovement()
    {
        // Apply force to move the player in the slide direction
        playerRigidbody.AddForce(locomotionParameters.GetSlideParameters().slideSpeed * Time.deltaTime * slideDirection);
        // Decrease slide timer if not on a slope
        if (!slopeDetector.GetSlopeDetectionFeedback().isOnSlope)
        {
            slideTimer -= Time.deltaTime;
        }
    }

    // Function to stop sliding
    void StopSlide()
    {
        // Restore player collider height
        playerCollider.height = 2;
        // Reset sliding flag
        isSliding = false;
        // Reset slide timer
        slideTimer = locomotionParameters.GetSlideParameters().slideDuration;
    }
}
