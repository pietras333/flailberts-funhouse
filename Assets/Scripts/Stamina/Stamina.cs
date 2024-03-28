using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [Header("Stamina")]
    [Space]
    [Header("Scripts")]
    [SerializeField] InputReceiver inputReceiver;
    [SerializeField] GroundDetector groundDetector;
    [SerializeField] StaminaParameters staminaParameters; // Reference to the stamina parameters scriptable object
    [SerializeField] bool canRegenerateStamina; // Flag indicating whether stamina regeneration is enabled
    [HideInInspector] float currentStamina; // Current stamina value

    public void Start()
    {
        currentStamina = staminaParameters.GetStaminaParametersFeedback().stamina;
    }

    // Update is called once per frame
    public void Update()
    {
        // If stamina regeneration is enabled, handle stamina regeneration
        if (canRegenerateStamina)
        {
            HandleStaminaRegeneration();
        }
    }

    // Function to handle stamina regeneration
    void HandleStaminaRegeneration()
    {
        // Check if current stamina is less than maximum stamina
        if ((currentStamina < staminaParameters.GetStaminaParametersFeedback().stamina) && !inputReceiver.GetInputFeedback().isRunning)
        {
            // Evaluate the animation curve to determine regeneration rate based on stamina percentage
            float staminaPercentage = currentStamina / staminaParameters.GetStaminaParametersFeedback().stamina;
            float regenerationRate = staminaParameters.GetStaminaParametersFeedback().regenerationCurve.Evaluate(staminaPercentage);

            // Increment current stamina based on regeneration rate and time
            currentStamina += Time.deltaTime * regenerationRate * staminaParameters.GetStaminaParametersFeedback().staminaRegeneration;

            // Clamp current stamina to ensure it doesn't exceed maximum stamina
            currentStamina = Mathf.Clamp(currentStamina, 0, staminaParameters.GetStaminaParametersFeedback().stamina);
        }
    }

    public void DecreaseStamina(float staminaCost)
    {
        currentStamina -= staminaCost;
        canRegenerateStamina = false;
        Invoke(nameof(AllowStaminaRegeneration), staminaParameters.GetStaminaParametersFeedback().staminaRegenerationCooldown);
    }

    public StaminaStateFeedback GetStaminaStateFeedback()
    {
        return new StaminaStateFeedback(currentStamina);
    }

    void AllowStaminaRegeneration()
    {
        canRegenerateStamina = true;
    }
}


public class StaminaStateFeedback
{
    public float currentStamina;

    public StaminaStateFeedback(float currentStamina)
    {
        this.currentStamina = currentStamina;
    }
}