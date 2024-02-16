using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelebrationHandler : MonoBehaviour
{
    [Header("Celebration Handler")]
    [Space]
    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] Movement movement;
    [Space]
    [Header("Configuration")]
    [SerializeField] int animationsCount;
    [SerializeField] float animationDuration = 3;


    void Start()
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        if (!animator || !movement)
        {
            Debug.LogError("One or more references are missing in the CelebrationHandler script.", gameObject);
            return;
        }
    }

    public void PlayRandomAnimation()
    {
        int animationIndex = Random.Range(0, animationsCount);
        animator.SetInteger("celebrationIndex", animationIndex);
        movement.canMove = false;
        Invoke("ResetAnimatorState", animationDuration);
    }

    void ResetAnimatorState()
    {
        animator.SetInteger("celebrationIndex", -1);
        movement.canMove = true;
    }
}
