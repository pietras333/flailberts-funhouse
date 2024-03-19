using System.Collections;
using UnityEngine;

public class Locomotion : MonoBehaviour
{
    [Header("Locomotion")]
    [Space]
    [Header("References")]
    [SerializeField] Rigidbody playerRigidbody; // Reference to the player's Rigidbody component
    [Header("Scripts")]
    [SerializeField] LocomotionParameters locomotionParameters; // Scriptable object holding locomotion parameters
    [SerializeField] InputReceiver inputReceiver; // Script handling player input
    [SerializeField] GroundDetector groundDetector; // Script detecting if the player is grounded
    [SerializeField] SlopeDetector slopeDetector; // Script detecting if the player is on a slope
    [SerializeField] SlideHandler slideHandler;

    void Update()
    {
        // Enable/disable gravity based on whether the player is on a slope
        playerRigidbody.useGravity = !slopeDetector.GetSlopeDetectionFeedback().isOnSlope;

        // Set drag based on whether the player is grounded
        playerRigidbody.drag = groundDetector.GetGroundFeedback().isGrounded ? locomotionParameters.GetAdditionalParameters().groundDrag : locomotionParameters.GetAdditionalParameters().airDrag;
    }

    void FixedUpdate()
    {
        // Handle movement, speed control, and jumping in FixedUpdate for physics calculations
        HandleSpeedControl();
        HandleMovement();
        HandleJump();
    }

    // Handle player movement
    void HandleMovement()
    {
        // Apply custom gravity
        playerRigidbody.AddForce(-locomotionParameters.GetAdditionalParameters().gravity * Time.fixedDeltaTime * transform.up, ForceMode.Force);

        // If the player is sliding, skip applying movement force
        if (slideHandler.isSliding)
        {
            return;
        }

        // Apply force for movement
        playerRigidbody.AddForce(GetCurrentSpeed() * Time.fixedDeltaTime * GetCurrentDirection());
    }

    // Get the current speed based on player input and state
    float GetCurrentSpeed()
    {
        if (groundDetector.GetGroundFeedback().isGrounded && !inputReceiver.GetInputFeedback().isRunning)
        {
            return locomotionParameters.GetWalkParameters().walkSpeed;
        }
        return locomotionParameters.GetRunParameters().runSpeed;
    }

    // Get the current movement direction based on player input and slope state
    public Vector3 GetCurrentDirection()
    {
        if (!slopeDetector.GetSlopeDetectionFeedback().isOnSlope)
        {
            return inputReceiver.GetInputFeedback().direction;
        }
        return slopeDetector.GetSlopeDetectionFeedback().slopeDirection;
    }

    // Get the maximum speed based on player input and state
    float GetCurrentMaxSpeed()
    {
        if (groundDetector.GetGroundFeedback().isGrounded && !inputReceiver.GetInputFeedback().isRunning)
        {
            return locomotionParameters.GetWalkParameters().maxWalkSpeed;
        }
        return locomotionParameters.GetRunParameters().maxRunSpeed;
    }

    // Handle player jumping
    void HandleJump()
    {
        if (inputReceiver.GetInputFeedback().isJumping && groundDetector.GetGroundFeedback().isGrounded)
        {
            // Zero out vertical velocity before jumping
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
            // Apply jump force
            playerRigidbody.AddForce(locomotionParameters.GetJumpParameters().jumpForce * transform.up, ForceMode.Force);
        }
    }

    // Handle controlling player speed
    void HandleSpeedControl()
    {
        // Smoothly stop the player if not moving and grounded
        if (groundDetector.GetGroundFeedback().isGrounded && inputReceiver.GetInputFeedback().direction == Vector3.zero)
        {
            playerRigidbody.velocity = Vector3.Lerp(playerRigidbody.velocity, Vector3.zero, locomotionParameters.GetAdditionalParameters().stopSmothness * Time.fixedDeltaTime);
        }

        // Limit speed when on a slope
        if (slopeDetector.GetSlopeDetectionFeedback().isOnSlope && !inputReceiver.GetInputFeedback().isJumping)
        {
            if (playerRigidbody.velocity.magnitude > GetCurrentMaxSpeed())
            {
                playerRigidbody.velocity = playerRigidbody.velocity.normalized * GetCurrentMaxSpeed();
            }
        }
        else
        {
            // Limit speed when not on a slope
            Vector3 velocityIsolated = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
            if (velocityIsolated.magnitude > GetCurrentMaxSpeed())
            {
                Vector3 velocityControlled = velocityIsolated.normalized * GetCurrentMaxSpeed();
                playerRigidbody.velocity = new Vector3(velocityControlled.x, playerRigidbody.velocity.y, velocityControlled.z);
            }
        }
    }
}
