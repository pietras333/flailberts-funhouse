using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class AttackDetector : MonoBehaviour
{
    [Header("Attack Detector")]
    [Space]
    [Header("References")]
    [Header("Scripts")]
    [SerializeField] CameraShakeHandler cameraShakeHandler;
    [SerializeField] Combat combat; // Reference to the Combat script
    [SerializeField] CombatParameters combatParameters; // Reference to the CombatParameters script

    // Called when another collider enters the trigger collider attached to this object
    public void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider's tag matches the enemy tag and the player is attacking
        if (other.CompareTag(combatParameters.GetCombatParametersFeedback().enemyTag) && combat.GetCombatStateFeedback().isAttacking)
        {
            // If conditions are met, damage the enemy's parent object's health
            other.GetComponentInParent<Health>().GetDamage(combatParameters.GetCombatParametersFeedback().damage);
            cameraShakeHandler.ShakeOnAttack();
        }
    }
}
