using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorHub : MonoBehaviour
{
    [Header("Player Animator Hub")]
    [Header("References")]
    [Header("Scripts")]
    [SerializeField] InputHandler inputHandler;
    [SerializeField] GroundDetection groundDetection;
    [SerializeField] Locomotion locomotion;
    [Header("Objects")]
    [SerializeField] Animator animator;
    void Update()
    {
        animator.SetBool("isRunning", inputHandler.CheckForMovement());
        animator.SetBool("isSprinting", inputHandler.isRunning);
        animator.SetBool("isGrounded", groundDetection.CheckGrounded().checkForGround);
        animator.SetBool("isSliding", locomotion.isSliding);
    }
}
