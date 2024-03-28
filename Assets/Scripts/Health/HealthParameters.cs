using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthParameters : MonoBehaviour
{
    [Header("Health Parameters")]
    [Space]
    [Header("Configuration")]

    // Initial health value
    [SerializeField, Range(1, 10)] float health = 5f;

    // Rate at which health regenerates
    [SerializeField, Range(1, 10f)] float healthRegeneration = 6f;
    [SerializeField, Range(0, 5f)] float healthRegenerationCooldown = 0.75f;

    // Animation curve defining the regeneration rate over time
    [SerializeField] AnimationCurve healthRegenerationCurve;

    // Global multiplier for health parameters
    [SerializeField] float globalMultiplier = 10f;

    // Function to get health parameters feedback
    public HealthParametersFeedback GetHealthParametersFeedback()
    {
        // Return health parameters feedback based on configured values
        return new HealthParametersFeedback(health * globalMultiplier, healthRegeneration, healthRegenerationCooldown, healthRegenerationCurve);
    }

}

// Class to hold health parameters feedback
public class HealthParametersFeedback
{
    public float health; // Current health value
    public float healthRegeneration; // Rate at which health regenerates
    public float healthRegenerationCooldown; // Rate at which health regenerates
    public AnimationCurve regenerationCurve; // Animation curve defining the regeneration rate over time

    // Constructor to initialize health parameters feedback
    public HealthParametersFeedback(float health, float healthRegeneration, float healthRegenerationCooldown, AnimationCurve regenerationCurve)
    {
        this.health = health;
        this.healthRegeneration = healthRegeneration;
        this.healthRegenerationCooldown = healthRegenerationCooldown;
        this.regenerationCurve = regenerationCurve;
    }
}
