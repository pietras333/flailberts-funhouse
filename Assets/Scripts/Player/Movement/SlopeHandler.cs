using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlopeHandler : MonoBehaviour
{
    [Header("Slope Handler")]

    [Header("References")]

    [Header("Scripts")]
    [SerializeField] GroundDetection groundDetection;
    [SerializeField] InputHandler inputHandler;

    [Header("Configuration")]
    [SerializeField] float maxSlopeAngle = 45f;

    public SlopeDetectionResult CheckForSlope()
    {
        GroundDetectionResult checkResult = groundDetection.CheckGrounded();
        RaycastHit groundHit = checkResult.hitInfo;

        float angle = Vector3.Angle(Vector3.up, groundHit.normal);
        bool checkForSlope = angle <= maxSlopeAngle && angle != 0;
        if (checkForSlope)
        {
            Vector3 slopeMoveDirection = Vector3.ProjectOnPlane(inputHandler.direction, groundHit.normal).normalized;
            Debug.Log("slopeMoveDirection: " + slopeMoveDirection);
            return new SlopeDetectionResult(checkForSlope, slopeMoveDirection);
        }
        return new SlopeDetectionResult(false, Vector3.zero);
    }
}


public class SlopeDetectionResult
{
    public bool checkForSlope;
    public Vector3 slopeMoveDirection;
    public SlopeDetectionResult(bool checkForSlope, Vector3 slopeMoveDirection)
    {
        this.checkForSlope = checkForSlope;
        this.slopeMoveDirection = slopeMoveDirection;
    }
}
