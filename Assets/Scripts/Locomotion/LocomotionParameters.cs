using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LocomotionParameters : MonoBehaviour
{
    [Header("Locomotion Parameters")]
    [Space]
    [Header("Global multipliers")]
    [SerializeField] float globalMultiplier = 100f; // Global multiplier for parameters
    [SerializeField] float movementMultiplier = 1000f; // Multiplier for movement parameters
    [SerializeField] float stopSmothness = 10f; // Smoothing factor for stopping
    [SerializeField] float gravity = 20f; // Custom gravity value
    [SerializeField] float groundDrag = 0f; // Ground drag value
    [SerializeField] float airDrag = 1f; // Air drag value

    [Header("Walk configuration")]
    [SerializeField, Range(1, 10)] float walkSpeed = 6f; // Walking speed

    [Header("Run configuration")]
    [SerializeField, Range(8, 15)] float runSpeed = 11f; // Running speed

    [Header("Jump configuration")]
    [SerializeField, Range(1, 5)] float jumpForce = 4.5f; // Jump force

    // Function to get walk parameters
    public WalkParameters GetWalkParameters()
    {
        return new WalkParameters(walkSpeed * movementMultiplier, walkSpeed);
    }

    // Function to get run parameters
    public RunParameters GetRunParameters()
    {
        return new RunParameters(runSpeed * movementMultiplier, walkSpeed);
    }

    // Function to get jump parameters
    public JumpParameters GetJumpParameters()
    {
        return new JumpParameters(jumpForce * globalMultiplier);
    }

    // Function to get additional parameters
    public AdditionalParameters GetAdditionalParameters()
    {
        return new AdditionalParameters(stopSmothness, gravity, groundDrag, airDrag);
    }
}

// Class to hold walk parameters
public class WalkParameters
{
    public float walkSpeed; // Walking speed
    public float maxWalkSpeed; // Maximum walking speed
    public WalkParameters(float walkSpeed, float maxWalkSpeed)
    {
        this.walkSpeed = walkSpeed;
        this.maxWalkSpeed = maxWalkSpeed;
    }
}

// Class to hold run parameters
public class RunParameters
{
    public float runSpeed; // Running speed
    public float maxRunSpeed; // Maximum running speed
    public RunParameters(float runSpeed, float maxRunSpeed)
    {
        this.runSpeed = runSpeed;
        this.maxRunSpeed = maxRunSpeed;
    }
}

// Class to hold jump parameters
public class JumpParameters
{
    public float jumpForce; // Jump force
    public JumpParameters(float jumpForce)
    {
        this.jumpForce = jumpForce;
    }
}

// Class to hold additional parameters
public class AdditionalParameters
{
    public float stopSmothness; // Smoothing factor for stopping
    public float gravity; // Custom gravity value
    public float groundDrag; // Ground drag value
    public float airDrag; // Air drag value
    public AdditionalParameters(float stopSmothness, float gravity, float groundDrag, float airDrag)
    {
        this.stopSmothness = stopSmothness;
        this.gravity = gravity;
        this.groundDrag = groundDrag;
        this.airDrag = airDrag;
    }
}
