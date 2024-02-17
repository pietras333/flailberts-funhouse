using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickHandler : MonoBehaviour
{
    [Header("Kick Handler")]
    [Space]
    [Header("References")]
    [SerializeField] Movement movement;
    [SerializeField] Animator animator;
    [SerializeField] BallControl ballControl;
    [Space]
    [Header("Configuration")]
    [SerializeField] KeyCode kickKey = KeyCode.F;
    [SerializeField] public float kickCooldown = 0.25f;
    [HideInInspector] float kickCooldownHalf;
    [SerializeField] public float kickForce = 10f;
    [SerializeField, Range(1, 2)] float kickForceMultiplier = 2f;
    [SerializeField] public float kickForceAccumulator = 0.65f;
    [Space]
    [Header("Up Force")]
    [SerializeField] KeyCode upForceKey = KeyCode.Space;
    [SerializeField] public float upForceAccumulator = 0.5f;
    [SerializeField] public float upForce = 10f;
    [SerializeField] float currentUpForce;
    [Space]
    [Header("Detection")]
    [SerializeField] LayerMask ballLayer;
    [SerializeField] float kickDetectionRange = 0.85f;
    [SerializeField] float kickDetectionOffset = 0.5f;
    [Space]
    [Header("States")]
    [SerializeField] float currentKickForce;
    [SerializeField] public bool isKicking;
    [SerializeField] bool canShowKick;
    [SerializeField] public bool isUpForce;

    void Start()
    {
        kickCooldownHalf = kickCooldown * 0.5f;
        InitializeComponents();
    }

    void InitializeComponents()
    {
        if (!movement || !ballControl || !animator)
        {
            Debug.LogError("One or more references are missing in the KickHandler script.", gameObject);
            return;
        }
    }

    void Update()
    {
        CheckKickingState();

        CheckUpForceState();

        ClampForces();

        animator.SetBool("isKicking", canShowKick);
    }

    void CheckKickingState()
    {
        if (Input.GetKeyDown(kickKey) && !isKicking)
        {
            isKicking = true;
            StartCoroutine("KickForceIncrementation");
        }
        if (Input.GetKeyUp(kickKey) && isKicking)
        {
            ballControl.canControlBall = false;
            movement.canMove = false;
            canShowKick = true;

            StopCoroutine("KickForceIncrementation");

            Invoke("HandleKick", kickCooldownHalf);
            Invoke("StopKicking", kickCooldownHalf);
            Invoke("AllowBallControl", kickCooldown);
        }
    }

    void StopKicking()
    {
        isKicking = false;
        canShowKick = false;
        movement.canMove = true;
    }

    void HandleKick()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + movement.lastDirection - Vector3.up * kickDetectionOffset, kickDetectionRange, ballLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody ball = colliders[i].transform.gameObject.GetComponent<Rigidbody>();
            if (!ball)
            {
                Debug.LogError("Ball Rigidbody not detected!", gameObject);
                return;
            }
            if (!isUpForce)
            {
                ball.AddForce(this.transform.forward * currentKickForce * kickForceMultiplier, ForceMode.Impulse);
                return;
            }
            ball.AddForce(this.transform.forward * currentKickForce * kickForceMultiplier + Vector3.up * currentUpForce, ForceMode.Impulse);

        }
        currentUpForce = 0;
        currentKickForce = 0;
    }

    IEnumerator KickForceIncrementation()
    {
        while (currentKickForce < kickForce)
        {
            currentKickForce += kickForceAccumulator;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    void AllowBallControl()
    {
        ballControl.canControlBall = true;
    }

    void CheckUpForceState()
    {
        if (Input.GetKeyDown(upForceKey) && !isUpForce)
        {
            isUpForce = true;
            StartCoroutine("upForceIncrementation");
        }
        if (Input.GetKeyUp(upForceKey) && isUpForce)
        {
            StopCoroutine("upForceIncrementation");
            stopUpForce();
        }
    }

    IEnumerator upForceIncrementation()
    {
        while (currentUpForce < upForce)
        {
            currentUpForce += upForceAccumulator;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    void stopUpForce()
    {
        currentUpForce = 0;
        isUpForce = false;
    }

    void ClampForces()
    {
        currentUpForce = Mathf.Clamp(currentUpForce, 0, upForce);
        currentKickForce = Mathf.Clamp(currentKickForce, 0, kickForce);
    }
}
