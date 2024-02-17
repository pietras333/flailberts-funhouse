using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityProgressionManager : MonoBehaviour
{
    [Header("Ability Progression Manager")]
    [Space]
    [Header("References")]
    [SerializeField] KickHandler kickHandler;
    [SerializeField] RunHandler runHandler;
    [SerializeField] TackleHandler tackleHandler;
    [SerializeField] Movement movement;
    [Space]
    [Header("Kicking")]
    [SerializeField] int kickLevel = 1;
    [SerializeField] float kickForceEnhancement = 1.25f;
    [Space]
    [Header("Tackling")]
    [SerializeField] int tackleLevel = 1;
    [SerializeField] float tackleForceEnhancement = 1.05f;
    [Space]
    [Header("Running")]
    [SerializeField] int runLevel = 1;
    [SerializeField] float runMaxSpeedEnchancemenet = 1.1f;
    [SerializeField] float runStaminaEnchancement = 1.025f;
    [Header("Movement")]
    [SerializeField] int movementLevel = 1;
    [SerializeField] float movementSpeedEnchancement = 1.15f;
    [SerializeField] float movementMaxSpeedEnchancement = 1.05f;

    void Start()
    {
        InitializeComponents();
        HandleKickEnhancement();
        HandleTackleEnhancement();
        HandleRunEnchancement();
        HandleMovementEnchancement();
    }

    void InitializeComponents()
    {
        if (!kickHandler || !runHandler || !tackleHandler || !movement)
        {
            Debug.LogError("One or more references are missing in the AbilityProgressionManager script.", gameObject);
            return;
        }
    }

    void HandleKickEnhancement()
    {
        kickHandler.kickForce = kickHandler.kickForce * kickLevel * kickForceEnhancement;
    }

    void HandleTackleEnhancement()
    {
        tackleHandler.tackleForce = tackleHandler.tackleForce * tackleLevel * tackleForceEnhancement;
    }

    void HandleRunEnchancement()
    {
        runHandler.maxRunSpeed = runHandler.maxRunSpeed * runMaxSpeedEnchancemenet * runLevel;
        runHandler.maxStamina = runHandler.maxStamina * runStaminaEnchancement * runLevel;
    }

    void HandleMovementEnchancement()
    {
        movement.speed = movement.speed * movementSpeedEnchancement * movementLevel;
        movement.maxSpeed = movement.maxSpeed * movementMaxSpeedEnchancement * movementLevel;
    }
}
