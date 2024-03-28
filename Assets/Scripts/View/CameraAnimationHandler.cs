using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class CameraAnimationHandler : MonoBehaviour
{
    [SerializeField] Animator cameraAnimator;
    [SerializeField] InputReceiver inputReceiver;
    [SerializeField] GroundDetector groundDetector;
    [SerializeField] Rigidbody playerRigidbody;

    public void Update()
    {

        if (inputReceiver.GetInputFeedback().isJumping && groundDetector.GetGroundFeedback().isGrounded)
        {
            cameraAnimator.SetBool("isJumping", true);
        }
        else
        {
            cameraAnimator.SetBool("isJumping", false);
        }


        if (playerRigidbody.velocity.y < -1f && groundDetector.GetGroundFeedback().isGrounded)
        {
            cameraAnimator.SetBool("isFalling", true);
        }
        else
        {
            cameraAnimator.SetBool("isFalling", false);
        }
    }
}
