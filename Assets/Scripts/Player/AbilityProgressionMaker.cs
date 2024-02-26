using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityProgressionManager : MonoBehaviour
{
    [Header("Ability Progression Manager")]
    [Space]
    [Header("Initialization")]
    [SerializeField] float timeToInitialize = 1f;
    [Space]
    [Header("References")]
    [SerializeField] KickHandler kickHandler;
    [SerializeField] RunHandler runHandler;
    [SerializeField] TackleHandler tackleHandler;
    [SerializeField] Movement movement;
    [Space]
    [Header("Kicking")]
    [SerializeField] int kickLevel = 1;
    [SerializeField] float kickForceEnhancement = 0.25f;
    [Space]
    [Header("Tackling")]
    [SerializeField] int tackleLevel = 1;
    [SerializeField] float tackleForceEnhancement = 0.05f;
    [Space]
    [Header("Running")]
    [SerializeField] int runLevel = 1;
    [SerializeField] float runMaxSpeedEnchancemenet = 0.1f;
    [SerializeField] float runStaminaEnchancement = 0.025f;
    [Header("Movement")]
    [SerializeField] int movementLevel = 1;
    [SerializeField] float movementSpeedEnchancement = 0.15f;
    [SerializeField] float movementMaxSpeedEnchancement = 0.05f;
    [Header("Dribbling")]
    [SerializeField] int dribblingLevel = 1;
    [SerializeField] float dribblingSpeedEnchancement = 0.05f;
    [SerializeField] float dribblingMaxSpeedEnchancement = 0.1f;
    void Start()
    {
        InitializeComponents();
        StartCoroutine(LateStart(timeToInitialize));
    }

    IEnumerator LateStart(float timeToWait)
    {
        yield return new WaitForSecondsRealtime(timeToWait);
        InitializeVariables();
    }

    void InitializeVariables()
    {
        HandleKickEnhancement();
        HandleTackleEnhancement();
        HandleRunEnchancement();
        HandleMovementEnchancement();
        HandleDribblingEnchancement();
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
        kickHandler.kickForce += kickHandler.kickForce * kickLevel * kickForceEnhancement;
    }

    void HandleTackleEnhancement()
    {
        tackleHandler.tackleForce += tackleHandler.tackleForce * tackleLevel * tackleForceEnhancement;
    }

    void HandleRunEnchancement()
    {
        runHandler.maxRunSpeed += runHandler.maxRunSpeed * runMaxSpeedEnchancemenet * runLevel;
        runHandler.maxStamina += runHandler.maxStamina * runStaminaEnchancement * runLevel;
    }

    void HandleMovementEnchancement()
    {
        movement.speed += movement.speed * movementSpeedEnchancement * movementLevel;
        movement.maxSpeed += movement.maxSpeed * movementMaxSpeedEnchancement * movementLevel;
    }

    void HandleDribblingEnchancement()
    {
        movement.dribbleSpeed += movement.dribbleSpeed * dribblingSpeedEnchancement * dribblingLevel;
        movement.dribbleMaxSpeed -= movement.dribbleMaxSpeed * dribblingMaxSpeedEnchancement * dribblingLevel;
    }
}