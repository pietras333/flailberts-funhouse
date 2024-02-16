using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScoring : MonoBehaviour
{
    [Header("Goal Scoring")]
    [Space]
    [Header("References")]
    [SerializeField] GameManager gameManager;
    [SerializeField] Transform ballTransform;
    [SerializeField] Collider goalCollider;
    [SerializeField] List<ParticleSystem> confettiParticles = new List<ParticleSystem>();
    [Space]
    [Header("Configuration")]
    [SerializeField] float ballDetectionThreshold = 1f;
    [SerializeField] float gameResetCooldown = 8f;
    [SerializeField] string ballTag = "Ball";
    [Header("States")]
    [SerializeField] bool isScored;

    public void OnTriggerStay(Collider otherCollider)
    {
        HandleScoring(otherCollider);
    }

    void Start()
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        if (!gameManager || !ballTransform || !goalCollider || confettiParticles.Count == 0)
        {
            Debug.LogError("One or more references are missing in the GoalScoring script.", gameObject);
            return;
        }
    }

    private bool IsBallInGoalBounds(Collider ballCollider)
    {
        bool isInBounds = !(goalCollider.bounds.Contains(ballCollider.bounds.min * ballDetectionThreshold) && goalCollider.bounds.Contains(ballCollider.bounds.max * ballDetectionThreshold));
        if (isInBounds)
        {
            return false;
        }
        return true;
    }

    private void HandleScoring(Collider collider)
    {
        bool isInBounds = IsBallInGoalBounds(collider);
        if (!isScored && collider.tag == ballTag && isInBounds)
        {
            isScored = true;

            BallData ballData = collider.GetComponent<BallData>();
            GameObject scorer = ballData.lastTouchedBy;

            scorer.GetComponent<CelebrationHandler>().PlayRandomAnimation();
            PlayConfetti();
            ResetGameAfterCooldown();
        }
    }

    private void ResetGameAfterCooldown()
    {
        if (gameResetCooldown <= 0)
        {
            Debug.LogWarning("Game reset cooldown is not set or invalid.", gameObject);
            return;
        }
        Invoke("HandleGameReset", gameResetCooldown);
        Rigidbody ballRigidbody = ballTransform.GetComponent<Rigidbody>();
        ballRigidbody.velocity = Vector3.zero;
    }

    void PlayConfetti()
    {
        foreach (ParticleSystem confetti in confettiParticles)
        {
            confetti.Play();
        }
        isScored = true;
    }

    void HandleGameReset()
    {
        ballTransform.position = new Vector3(0, 10, 0);
        gameManager.ResetPlayersPosition();
        isScored = false;
    }
}
