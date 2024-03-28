using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaminaParameters : MonoBehaviour
{
    [Header("Stamina Parameters")] // Changed "Health Parameters" to "Stamina Parameters"
    [Space]
    [Header("Configuration")]

    [SerializeField, Range(1, 10)] float stamina = 5f; // Initial stamina value
    [SerializeField, Range(1, 10f)] float staminaRegeneration = 6f; // Rate at which stamina regenerates
    [SerializeField, Range(0, 5f)] float staminaRegenerationCooldown = 1f;
    [SerializeField] AnimationCurve staminaRegenerationCurve; // Animation curve defining the regeneration rate over time
    [SerializeField] float globalMultiplier = 10f; // Global multiplier for stamina parameters

    // Function to get stamina parameters feedback
    public StaminaParametersFeedback GetStaminaParametersFeedback()
    {
        // Return stamina parameters feedback based on configured values
        return new StaminaParametersFeedback(stamina * globalMultiplier, staminaRegeneration, staminaRegenerationCooldown, staminaRegenerationCurve);
    }

}

// Class to hold stamina parameters feedback
public class StaminaParametersFeedback
{
    public float stamina; // Current stamina value
    public float staminaRegeneration; // Rate at which stamina regenerates
    public float staminaRegenerationCooldown; // Rate at which stamina regenerates
    public AnimationCurve regenerationCurve; // Animation curve defining the regeneration rate over time

    // Constructor to initialize stamina parameters feedback
    public StaminaParametersFeedback(float stamina, float staminaRegeneration, float staminaRegenerationCooldown, AnimationCurve regenerationCurve)
    {
        this.stamina = stamina;
        this.staminaRegeneration = staminaRegeneration;
        this.staminaRegenerationCooldown = staminaRegenerationCooldown;
        this.regenerationCurve = regenerationCurve;
    }
}
