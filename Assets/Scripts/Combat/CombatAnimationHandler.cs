using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationHandler : MonoBehaviour
{
    [Header("Combat Animation Handler")]

    [Header("Scripts")]
    [SerializeField] GroundDetector groundDetector;

    [SerializeField] InputReceiver inputReceiver;
    [Header("References")]
    [SerializeField] Animator combatAnimator;

    void Update()
    {
        combatAnimator.SetBool("isRunning", inputReceiver.GetInputFeedback().isRunning);
        combatAnimator.SetBool("isGrounded", groundDetector.GetGroundFeedback().isGrounded);
        combatAnimator.SetBool("isWalking", inputReceiver.GetInputFeedback().direction != Vector3.zero);
    }
}
