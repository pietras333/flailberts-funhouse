using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatParameters : MonoBehaviour
{
    [Header("Combat Parameters")]
    [Space]
    [Header("Configuration")]

    // Time window for combo attacks
    [SerializeField, Range(0, 2)] float timeForCombo = 1f;

    // Base damage dealt by attacks
    [SerializeField, Range(1, 10)] float damage = 2f;

    // Global multiplier applied to damage
    [SerializeField] float globalMultiplier = 10f;

    // Tag for identifying enemies
    [SerializeField] string enemyTag = "Enemy";

    // Function to retrieve combat parameters feedback
    public CombatParametersFeedback GetCombatParametersFeedback()
    {
        // Return a new instance of CombatParametersFeedback with configured parameters
        return new CombatParametersFeedback(timeForCombo, damage * globalMultiplier, enemyTag);
    }
}

// Class to hold combat parameters feedback
public class CombatParametersFeedback
{
    public float timeForCombo; // Time window for combo attacks
    public float damage; // Damage dealt by attacks
    public string enemyTag; // Tag for identifying enemies

    // Constructor to initialize combat parameters feedback
    public CombatParametersFeedback(float timeForCombo, float damage, string enemyTag)
    {
        this.timeForCombo = timeForCombo; // Assign time for combo
        this.damage = damage; // Assign damage
        this.enemyTag = enemyTag; // Assign enemy tag
    }
}
