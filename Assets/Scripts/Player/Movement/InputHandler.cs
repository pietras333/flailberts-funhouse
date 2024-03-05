using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("Input Handler")]

    [Header("References")]

    [Header("Scripts")]
    [SerializeField] Locomotion locomotion;

    [Header("Objects")]
    [SerializeField] Transform orientation;

    [Header("Direction")]
    [SerializeField] float horizontalInput;
    [SerializeField] float verticalInput;
    [SerializeField] public Vector3 direction;

    [Header("Run")]
    [SerializeField] KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] public bool isRunning;

    [Header("Jump")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] public bool isJumping;

    [Header("Slide")]
    [SerializeField] public KeyCode slideKey = KeyCode.LeftControl;

    void Update()
    {
        GetBasicInput();
        SetVariables();
    }

    void GetBasicInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void SetVariables()
    {
        direction = GetDirection();
        isRunning = CheckForInput(runKey);
        isJumping = CheckForInput(jumpKey);
        Debug.Log(isJumping);
    }

    Vector3 GetDirection()
    {
        return horizontalInput * orientation.right + verticalInput * orientation.forward;
    }

    public bool CheckForMovement()
    {
        return direction != Vector3.zero;
    }

    bool CheckForInput(KeyCode key)
    {
        return Input.GetKey(key);
    }
}
