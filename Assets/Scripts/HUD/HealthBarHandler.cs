using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    [Header("HealthBarHandler")]
    [Space]
    [Header("References")]
    [SerializeField] Slider healthBar;
    [Header("Scripts")]
    [SerializeField] Health health;
    [SerializeField] HealthParameters healthParameters;


    void Update()
    {
        healthBar.value = health.GetHealthStateFeedback().currentHealth / healthParameters.GetHealthParametersFeedback().health;
    }

}
