using System.Collections;
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
    [SerializeField, Range(0.01f, 10)] float runStaminaCost = 0.1f;

    [Header("Jump configuration")]
    [SerializeField, Range(1, 5)] float jumpForce = 4.5f; // Jump force
    [SerializeField, Range(0.01f, 10)] float jumpStaminaCost = 0.01f;

    [Header("Slide configuration")]
    [SerializeField, Range(0.1f, 1f)] float slideDuration = 0.65f; // Duration of sliding
    [SerializeField, Range(5, 15)] float slideSpeed = 11f; // Speed of sliding

    [Header("Vault configuration")]
    [SerializeField, Range(0.1f, 10f)] float vaultForce = 5f;

    // Function to get walk parameters
    public WalkParameters GetWalkParameters()
    {
        return new WalkParameters(walkSpeed * movementMultiplier, walkSpeed);
    }

    // Function to get run parameters
    public RunParameters GetRunParameters()
    {
        return new RunParameters(runSpeed * movementMultiplier, walkSpeed, runStaminaCost);
    }

    // Function to get jump parameters
    public JumpParameters GetJumpParameters()
    {
        return new JumpParameters(jumpForce * globalMultiplier, jumpStaminaCost * globalMultiplier);
    }

    // Function to get additional parameters
    public AdditionalParameters GetAdditionalParameters()
    {
        return new AdditionalParameters(stopSmothness, gravity, groundDrag, airDrag);
    }

    // Function to get slide parameters
    public SlideParameters GetSlideParameters()
    {
        return new SlideParameters(slideDuration, slideSpeed * movementMultiplier, slideSpeed);
    }

    public VaultParameters GetVaultParameters()
    {
        return new VaultParameters(vaultForce);
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
    public float runStaminaCost;
    public RunParameters(float runSpeed, float maxRunSpeed, float runStaminaCost)
    {
        this.runSpeed = runSpeed;
        this.maxRunSpeed = maxRunSpeed;
        this.runStaminaCost = runStaminaCost;
    }
}

// Class to hold jump parameters
public class JumpParameters
{
    public float jumpForce; // Jump force
    public float jumpStaminaCost;
    public JumpParameters(float jumpForce, float jumpStaminaCost)
    {
        this.jumpForce = jumpForce;
        this.jumpStaminaCost = jumpStaminaCost;
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

// Class to hold slide parameters
public class SlideParameters
{
    public float slideDuration; // Duration of sliding
    public float slideSpeed; // Speed of sliding
    public float maxSlideSpeed; // Maximum sliding speed
    public SlideParameters(float slideDuration, float slideSpeed, float maxSlideSpeed)
    {
        this.slideDuration = slideDuration;
        this.slideSpeed = slideSpeed;
        this.maxSlideSpeed = maxSlideSpeed;
    }
}

public class VaultParameters
{
    public float vaultForce;

    public VaultParameters(float vaultForce)
    {
        this.vaultForce = vaultForce;
    }
}
