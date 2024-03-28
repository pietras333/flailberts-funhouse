using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarHandler : MonoBehaviour
{
    [Header("StaminaBarHandler")]
    [Space]
    [Header("References")]
    [SerializeField] Slider staminaBar;
    [Header("Scripts")]
    [SerializeField] Stamina stamina;
    [SerializeField] StaminaParameters staminaParameters;


    void Update()
    {
        staminaBar.value = stamina.GetStaminaStateFeedback().currentStamina / staminaParameters.GetStaminaParametersFeedback().stamina;
    }

}
