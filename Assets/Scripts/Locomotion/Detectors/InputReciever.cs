using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class InputReceiver : MonoBehaviour
{
    [Header("Input")]
    [Space]
    [Header("References")]
    [Header("Scripts")]
    [SerializeField] ViewParameters viewParameters; // Reference to view parameters script
    [Header("Objects")]
    [SerializeField] Transform orientation; // Player orientation for movement direction

    [Header("Configuration")]

    [Header("Camera Rotation")]
    [SerializeField] float mouseX;
    [SerializeField] float mouseY;
    [SerializeField] float rotationX;
    [SerializeField] float rotationY;
    [SerializeField] float rotationZ;

    [Header("Direction")]
    [SerializeField] float horizontalInput;
    [SerializeField] float verticalInput;
    [SerializeField] Vector3 direction; // Current movement direction

    [Header("Run input")]
    [SerializeField] KeyCode runKey = KeyCode.LeftShift;

    [Header("Jump input")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Slide input")]
    [SerializeField] KeyCode slideKey = KeyCode.LeftControl; // Key for sliding

    [Header("Combat input")]
    [SerializeField] KeyCode attackKey = KeyCode.Mouse0;

    // Function to retrieve input feedback
    public InputFeedback GetInputFeedback()
    {
        bool isRunning = CheckForInput(runKey); // Check if running key is pressed
        bool isJumping = CheckForInput(jumpKey); // Check if jump key is pressed
        return new InputFeedback(direction, isRunning, isJumping); // Return input feedback
    }

    public ViewFeedback GetViewFeedback()
    {
        Quaternion viewRotation = Quaternion.Euler(rotationX, rotationY, rotationZ); // Create view rotation quaternion
        return new ViewFeedback(viewRotation, rotationX, rotationY); // Return view feedback
    }

    public InputParametersFeedback GetInputParametersFeedback()
    {
        return new InputParametersFeedback(slideKey, jumpKey); // Return slide key parameter feedback
    }

    public CombatFeedback GetCombatFeedback()
    {
        return new CombatFeedback(attackKey);
    }

    // Update is called once per frame
    void Update()
    {
        // Capture basic movement input
        CaptureBasicInput();

        // Update variables for further use
        UpdateVariables();
    }

    // Function to capture basic movement input
    void CaptureBasicInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); // Get horizontal input
        verticalInput = Input.GetAxisRaw("Vertical"); // Get vertical input

        mouseX = Input.GetAxisRaw("Mouse X"); // Get mouse X input
        mouseY = Input.GetAxisRaw("Mouse Y"); // Get mouse Y input
    }

    // Function to update variables based on input
    void UpdateVariables()
    {
        direction = CalculateDirection(); // Calculate movement direction

        rotationX -= mouseY * viewParameters.GetRotationParameters().sensitivity; // Update X rotation based on mouse Y input
        rotationX = Mathf.Clamp(rotationX, -viewParameters.GetRotationParameters().lockRotationX, viewParameters.GetRotationParameters().lockRotationX); // Clamp X rotation
        rotationY += mouseX * viewParameters.GetRotationParameters().sensitivity; // Update Y rotation based on mouse X input
        rotationZ = Mathf.Lerp(rotationZ, viewParameters.GetRotationParameters().cameraTiltZ * -horizontalInput, viewParameters.GetRotationParameters().tiltSmothness * Time.deltaTime); // Update Z rotation for camera tilt
    }

    // Function to calculate direction based on input and orientation
    Vector3 CalculateDirection()
    {
        Vector3 directionRaw = horizontalInput * orientation.right + verticalInput * orientation.forward; // Calculate raw direction based on input and orientation
        return directionRaw.normalized; // Return normalized direction
    }

    // Check if a specific key is pressed
    bool CheckForInput(KeyCode key)
    {
        return Input.GetKey(key); // Check if key is pressed and return result
    }
}

// Class to hold input feedback
public class InputFeedback
{
    public Vector3 direction; // Direction of movement
    public bool isRunning; // Flag indicating if running
    public bool isJumping; // Flag indicating if jumping

    // Constructor to initialize input direction
    public InputFeedback(Vector3 direction, bool isRunning, bool isJumping)
    {
        this.direction = direction; // Assign direction
        this.isRunning = isRunning; // Assign running flag
        this.isJumping = isJumping; // Assign jumping flag
    }
}

// Class to hold view feedback
public class ViewFeedback
{
    public Quaternion viewRotation; // Rotation of the view
    public float rotationX; // X rotation
    public float rotationY; // Y rotation

    // Constructor to initialize view rotation
    public ViewFeedback(Quaternion viewRotation, float rotationX, float rotationY)
    {
        this.viewRotation = viewRotation; // Assign view rotation
        this.rotationX = rotationX; // Assign X rotation
        this.rotationY = rotationY; // Assign Y rotation
    }
}

// Class to hold input parameters feedback
public class InputParametersFeedback
{
    public KeyCode slideKey; // Key for sliding

    public KeyCode jumpKey;

    // Constructor to initialize slide key
    public InputParametersFeedback(KeyCode slideKey, KeyCode jumpKey)
    {
        this.slideKey = slideKey; // Assign slide key
        this.jumpKey = jumpKey; // Assign slide key
    }
}


public class CombatFeedback
{
    public KeyCode attackKey;

    public CombatFeedback(KeyCode attackKey)
    {
        this.attackKey = attackKey;
    }
}
