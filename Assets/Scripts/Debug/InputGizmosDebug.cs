using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGizmosDebug : MonoBehaviour
{
    [Header("Input Gizmos Debug")]
    [Space]
    [Header("References")]
    [SerializeField] InputReceiver inputReceiver; // Reference to the input receiver script

    // Draw gizmos in the Scene view
    void OnDrawGizmos()
    {
        DebugDirection(); // Draw gizmos to visualize input direction
        DebugJump(); // Draw gizmos to visualize jump input
    }

    // Draw gizmos to visualize input direction
    void DebugDirection()
    {
        if (!inputReceiver.GetInputFeedback().isRunning)
        {
            Gizmos.color = Color.blue; // Set gizmo color to blue
            // Draw a line from the object's position to the position offset by the input direction
            Gizmos.DrawLine(transform.position, transform.position + inputReceiver.GetInputFeedback().direction);
        }
        else
        {
            Gizmos.color = Color.red; // Set gizmo color to red
            // Draw a line from the object's position to the position offset by the input direction multiplied by 2
            Gizmos.DrawLine(transform.position, transform.position + inputReceiver.GetInputFeedback().direction * 2f);
        }
    }

    // Draw gizmos to visualize jump input
    void DebugJump()
    {
        if (inputReceiver.GetInputFeedback().isJumping)
        {
            Gizmos.color = Color.green; // Set gizmo color to green
            // Draw a line from the object's position to the position offset upward
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2f);
        }
    }
}
