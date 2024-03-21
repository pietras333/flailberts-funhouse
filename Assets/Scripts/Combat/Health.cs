using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [Space]
    [Header("Scripts")]

    // Reference to the health parameters scriptable object
    [SerializeField] HealthParameters healthParameters;

    // Flag indicating whether health regeneration is enabled
    [SerializeField] bool canRegenerateHealth;

    // Current health value
    [HideInInspector] float currentHealth;

    // Update is called once per frame
    public void Update()
    {
        // If health regeneration is enabled, handle health regeneration
        if (canRegenerateHealth)
        {
            HandleHealthRegeneration();
        }
    }

    // Function to handle health regeneration
    void HandleHealthRegeneration()
    {
        // Check if current health is less than maximum health
        if (currentHealth < healthParameters.GetHealthParametersFeedback().health)
        {
            // Evaluate the animation curve to determine regeneration rate based on health percentage
            float healthPercentage = currentHealth / healthParameters.GetHealthParametersFeedback().health;
            float regenerationRate = healthParameters.GetHealthParametersFeedback().regenerationCurve.Evaluate(healthPercentage);

            // Debugging: Print out regeneration rate
            Debug.Log("Regeneration rate: " + regenerationRate);

            // Increment current health based on regeneration rate and time
            currentHealth += Time.deltaTime * regenerationRate * healthParameters.GetHealthParametersFeedback().healthRegeneration;

            // Clamp current health to ensure it doesn't exceed maximum health
            currentHealth = Mathf.Clamp(currentHealth, 0, healthParameters.GetHealthParametersFeedback().health);
        }
    }

    // Function to apply damage to the object
    public void GetDamage(float damage)
    {
        currentHealth -= damage; // Reduce current health by the specified damage amount
    }
}
