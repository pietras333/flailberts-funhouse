using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [Header("Combat")]
    [Space]
    [Header("References")]
    [SerializeField] Animator combatAnimator; // Reference to the Animator component
    [Header("Scripts")]
    [SerializeField] InputReceiver inputReceiver; // Reference to the InputReceiver script
    [SerializeField] CombatParameters combatParameters; // Reference to the CombatParameters script

    [HideInInspector] int comboIndex; // Index of the current combo
    [HideInInspector] float timeFromLastAttack = 0; // Time since the last attack

    private void Update()
    {
        HandleCombat(); // Handle player combat input
        HandleComboReset(); // Reset combo if time between attacks exceeds the allowed threshold
        HandleAnimatorState(); // Update animator state based on current combo index
    }

    // Update the Animator state based on the current combo index
    void HandleAnimatorState()
    {
        combatAnimator.SetInteger("ComboIndex", comboIndex);
    }

    // Handle player combat input
    void HandleCombat()
    {
        if (Input.GetKeyDown(inputReceiver.GetCombatInputFeedback().attackKey))
        {
            if (comboIndex == 2) // If the maximum combo index is reached, reset to zero
            {
                comboIndex = 0;
                return;
            }
            comboIndex++; // Increment combo index
            timeFromLastAttack = 0; // Reset time since last attack
        }
        timeFromLastAttack += Time.deltaTime; // Update time since last attack
    }

    // Reset combo if time between attacks exceeds the allowed threshold
    void HandleComboReset()
    {
        if (timeFromLastAttack >= combatParameters.GetCombatParametersFeedback().timeForCombo)
        {
            comboIndex = 0; // Reset combo index
        }
    }

    // Method to get feedback about the combat state
    public CombatStateFeedback GetCombatStateFeedback()
    {
        bool isAttacking = comboIndex != 0; // Player is attacking if combo index is not zero
        return new CombatStateFeedback(isAttacking);
    }
}

// Class to provide feedback about the combat state
public class CombatStateFeedback
{
    public bool isAttacking; // Flag indicating whether the player is currently attacking

    // Constructor to initialize the feedback
    public CombatStateFeedback(bool isAttacking)
    {
        this.isAttacking = isAttacking;
    }
}
