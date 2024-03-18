using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    [Header("View Controller")]
    [Space]
    [Header("References")]
    [Header("Scripts")]
    [SerializeField] InputReceiver inputReceiver; // Reference to the input receiver script
    [SerializeField] ViewParameters viewParameters; // Reference to the view parameters script
    [Header("Objects")]
    [SerializeField] Transform cameraHolder; // The object holding the camera
    [SerializeField] Transform orientation; // The object representing player orientation

    void Start()
    {
        // Lock cursor if specified in view parameters
        if (!viewParameters.lockCursor)
        {
            return;
        }
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor
        Cursor.visible = false; // Hide cursor
    }

    void Update()
    {
        SetCameraRotation(); // Set camera rotation based on input
        UpdateOrientation(); // Update player orientation based on input
    }

    // Set camera rotation based on input
    void SetCameraRotation()
    {
        cameraHolder.transform.rotation = inputReceiver.GetViewFeedback().viewRotation; // Set camera rotation
    }

    // Update player orientation based on input
    void UpdateOrientation()
    {
        orientation.transform.rotation = Quaternion.Euler(0, inputReceiver.GetViewFeedback().rotationY, 0f); // Update player orientation
    }
}
