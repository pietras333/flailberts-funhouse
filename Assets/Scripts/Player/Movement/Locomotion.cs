using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion : MonoBehaviour
{
    [Header("Locomotion")]

    [Header("References")]

    [Header("Scripts")]
    [SerializeField] InputHandler inputHandler;
    [SerializeField] GroundDetection groundDetection;
    [SerializeField] SlopeHandler slopeHandler;

    [Header("Objects")]
    [SerializeField] public Rigidbody playerRigidbody;
    [SerializeField] Transform orientation;

    [Header("Current States")]
    [SerializeField] public float currentSpeed;

    [Header("Basic Locomotion")]
    [SerializeField] bool isGrounded;
    [SerializeField] float walkSpeed = 13f;
    [SerializeField] float walkMaxSpeed = 6f;
    [SerializeField] float runSpeed = 15f;
    [SerializeField] float runMaxSpeed = 9f;
    [SerializeField] float airMaxSpeed = 50f;
    [SerializeField] float currentMaxSpeed;
    [SerializeField] float forceMultiplier = 100f;
    [SerializeField] Vector3 currentDirection;
    [HideInInspector] Vector3 lastDirection;

    [Header("Jump")]
    [SerializeField] float jumpForce = 5f;

    public void Update()
    {
        // Update variables on runtime
        UpdateVariables();

        // Control speed by clamping its velocity
        HandleSpeedControl();

        // Set rotation to face last direction
        SetRotation();

        // Use gravity only when player is not on slope to avoid bumping
        playerRigidbody.useGravity = !slopeHandler.CheckForSlope().checkForSlope;
    }

    public void FixedUpdate()
    {
        // Handle basic locomotion 
        HandleMotion();

        // Handle Jumping
        HandleJump();
    }

    #region MAIN FUNCTIONS
    void HandleMotion()
    {
        // Look for input and apply forces
        if (inputHandler.CheckForMovement())
        {
            // Move player with current force
            playerRigidbody.AddForce(currentSpeed * forceMultiplier * Time.fixedDeltaTime * currentDirection, ForceMode.Force);
        }

        if (!isGrounded)
        {
            playerRigidbody.AddForce(9f * Time.fixedDeltaTime * -orientation.up, ForceMode.Force);
        }
    }

    void HandleJump()
    {
        // Look for input and if player is on ground in the moment he's jumping
        if (inputHandler.isJumping && isGrounded)
        {
            // Move player upward with force
            playerRigidbody.AddForce(forceMultiplier * jumpForce * orientation.up, ForceMode.Force);
        }
    }

    void UpdateVariables()
    {
        // Get speed
        currentSpeed = GetCurrentSpeed();

        // Get current max speed
        currentMaxSpeed = GetCurrentMaxSpeed();

        // Check if player is on ground
        isGrounded = groundDetection.CheckGrounded().checkForGround;

        // Get current direction
        currentDirection = GetCurrentDirection();

    }

    void HandleSpeedControl()
    {
        // If player moves faster than his max speed then reduce his velocity to max velocity obtainable with this speed
        if (playerRigidbody.velocity.magnitude > currentMaxSpeed)
        {
            playerRigidbody.velocity = playerRigidbody.velocity.normalized * currentMaxSpeed;
        }

        // If player sents no input then isolate x and z axis and stop him on those axis
        if (!inputHandler.CheckForMovement())
        {
            Vector3 isolatedVelocity = new Vector3(0f, playerRigidbody.velocity.y, 0f);
            playerRigidbody.velocity = Vector3.Lerp(playerRigidbody.velocity, isolatedVelocity, Time.deltaTime);
        }
    }

    void SetRotation()
    {
        // Look for input
        if (inputHandler.CheckForMovement())
        {
            // Apply last known direction as last direction
            lastDirection = currentDirection;
        }

        // Calculate target rotation based on last direction 
        Quaternion targetRotation = Quaternion.LookRotation(lastDirection, Vector3.up);

        // Rotate player smoothly with .Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
    }

    #endregion MAIN FUNCIONS

    #region GET / SET FUNCTIONS
    float GetCurrentSpeed()
    {
        // If player is not on ground set his speed to run speed
        if (!isGrounded)
        {
            return runSpeed;
        }
        // Return runSpeed if player is running or walkSpeed if is walking
        return inputHandler.isRunning ? runSpeed : walkSpeed;
    }

    Vector3 GetCurrentDirection()
    {
        // Check for slope and return slope direction if detected if not basic then return basic direction
        Vector3 direction = slopeHandler.CheckForSlope().checkForSlope ? slopeHandler.CheckForSlope().slopeMoveDirection : inputHandler.direction;
        return direction.normalized;
    }

    float GetCurrentMaxSpeed()
    {
        // If player is not on ground set his speed to air max speed
        if (!isGrounded)
        {
            return airMaxSpeed;
        }
        // Return runMaxSpeed if player is running or walkMaxSpeed if is walking
        return inputHandler.isRunning ? runMaxSpeed : walkMaxSpeed;
    }

    #endregion GET / SET FUNCTIONS
}