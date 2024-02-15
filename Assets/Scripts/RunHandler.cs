using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunHandler : MonoBehaviour
{
    [Header("Run Handler")]
    [Space]
    [Header("References")]
    [SerializeField] Movement movement;
    [SerializeField] Animator animator;
    [Space]
    [Header("Configuration")]
    [SerializeField] float maxRunSpeed = 8f;
    [SerializeField] KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] float maxStamina = 20f;
    [SerializeField] float staminaDecreaseMultiplier = 0.01f;
    [SerializeField] float staminaIncreaseMultiplier = 0.0075f;
    [Space]
    [Header("States")]
    [SerializeField] float currentStamina;
    [SerializeField] bool isRunning;
    [HideInInspector] float initialMaxSpeed;

    void Start()
    {
        initialMaxSpeed = movement.maxSpeed;
        currentStamina = maxStamina;
    }

    void InitializeComponents()
    {
        if (!movement || !animator)
        {
            Debug.LogError("One or more references are missing in the RunHandler script.", gameObject);
            return;
        }

    }

    void Update()
    {
        CheckRunningState();

        UpdateRunningState();

        ClampStamina();

        animator.SetBool("isRunning", movement.direction != Vector3.zero);
    }

    void CheckRunningState()
    {
        if (Input.GetKeyDown(runKey) && currentStamina > 0)
        {
            isRunning = true;
        }
        else if (Input.GetKeyUp(runKey) || currentStamina <= 0f)
        {
            isRunning = false;
        }
    }
    void UpdateRunningState()
    {
        if (isRunning)
        {
            StopCoroutine("HandleStaminaIncrease");
            StartCoroutine("HandleStaminaDecrease");
            movement.maxSpeed = maxRunSpeed;
            return;
        }
        if (!Input.GetKeyDown(runKey))
        {
            StopCoroutine("HandleStaminaDecrease");
            StartCoroutine("HandleStaminaIncrease");
        }
        movement.maxSpeed = initialMaxSpeed;
    }

    void ClampStamina()
    {
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    IEnumerator HandleStaminaDecrease()
    {
        while (currentStamina > 0)
        {
            currentStamina -= staminaDecreaseMultiplier;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    IEnumerator HandleStaminaIncrease()
    {
        while (currentStamina < maxStamina)
        {
            currentStamina += staminaIncreaseMultiplier;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
}
