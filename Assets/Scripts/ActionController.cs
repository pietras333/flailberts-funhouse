using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [Header("Action Controller")]
    [Space]
    [Header("References")]
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Movement movement;
    [SerializeField] Animator animator;
    [SerializeField] LayerMask ballLayer;
    [HideInInspector] Rigidbody ball;
    [Space]
    [Space]
    [Header("Keycodes")]
    [Space]
    [Header("Running")]
    [SerializeField] float maxRunSpeed = 8f;
    [HideInInspector] float baseMaxSpeed;
    [SerializeField] bool isRunning;
    [SerializeField] KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] float maxStamina = 20f;
    [SerializeField] float currentStamina;
    [SerializeField] float staminaDecreaseMultiplier = 0.01f;
    [Space]
    [Header("Up force")]
    [SerializeField] bool isUpForce;
    [SerializeField] KeyCode upForceKey = KeyCode.Space;
    [SerializeField] float upForceMultiplier = 0.45f;
    [SerializeField] float upForce = 10f;
    [SerializeField] float currentUpForce;
    [Space]
    [Header("Kicking")]
    [SerializeField] float kickDetectionRange = 0.85f;
    [SerializeField] float kickDetectionOffset = 0.5f;
    [SerializeField] KeyCode kickKey = KeyCode.F;
    [SerializeField] float kickForce = 10f;
    [SerializeField] float kickCooldown = 0.25f;
    [SerializeField] float kickMultiplier = 0.65f;
    [SerializeField] float kickForceMultiplier = 2f;
    [SerializeField] float currentKickForce;
    [HideInInspector] public bool isKicking;
    [HideInInspector] bool canShowKick;
    [Space]
    [Header("Tackling")]
    [SerializeField] KeyCode tackleKey = KeyCode.G;
    [SerializeField] float tackleSpeed = 15f;
    [SerializeField] float tackleCooldown = 1f;
    [SerializeField] float tackleMultiplier = 1.5f;
    [SerializeField] float tackleForceMultiplier = 3f;
    [SerializeField] public float currentTackleSpeed;
    [HideInInspector] public bool isTackling;
    [HideInInspector] bool canShowTackle;
    [HideInInspector] Vector3 tackleDirection;
    [Space]
    [Header("Diving")]
    [SerializeField] KeyCode diveKey = KeyCode.B;
    [SerializeField] float diveForce = 5f;
    [SerializeField] float diveCooldown = 1f;
    [SerializeField] float diveMultiplier = 0.65f;
    [SerializeField] float currentDiveForce;
    [SerializeField] float divingForceMultiplier = 5f;
    [HideInInspector] public bool isDiving;
    [HideInInspector] bool canShowDive;
    [HideInInspector] Vector3 diveDirection;
    [Space]
    [SerializeField] KeyCode tackledDebugKey = KeyCode.L;

    // 
    // VISUAL DEBUGGING
    // 

    void debugKickDetection(){
        Gizmos.color = Color.green;
        Vector3 direction = this.transform.position + movement.lastDirection - Vector3.up * kickDetectionOffset;
        Gizmos.DrawSphere(direction, kickDetectionRange);
    }

    void debugVelocityVector(){
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward);
    }

    public void OnDrawGizmosSelected(){
        debugKickDetection();
        debugVelocityVector();
    }

    void Start(){
        baseMaxSpeed = movement.maxSpeed;
        currentStamina = maxStamina;
    }

    void Update(){
        //
        // Running
        if(Input.GetKeyDown(runKey) && currentStamina > 0){
            isRunning = true;
        }
        if(Input.GetKeyUp(runKey) || currentStamina <= 0f){
            isRunning = false;
        }

        if(isRunning){
            StopCoroutine("handleStaminaIncrease");
            StartCoroutine("handleStaminaDecrease");
            movement.maxSpeed = maxRunSpeed;
        }
        if(!isRunning){
            if(!Input.GetKeyDown(runKey)){
                StopCoroutine("handleStaminaDecrease");
                StartCoroutine("handleStaminaIncrease");                
            }
            movement.maxSpeed = baseMaxSpeed;
        }

        // 
        // Up-force switch
        if(Input.GetKeyDown(upForceKey) && !isUpForce){
            isUpForce = true;
            StartCoroutine("upForceIncrementation");
        }
        if(Input.GetKeyUp(upForceKey) && isUpForce){
            StopCoroutine("upForceIncrementation");
            resetUpForce();
        }

        // 
        // Kicking
        if(Input.GetKeyDown(kickKey) && !isKicking){
            isKicking = true;
            StartCoroutine("kickForceIncrementation");
        }
        if(Input.GetKeyUp(kickKey) && isKicking){
            movement.canMove = false;
            canShowKick = true;
            StopCoroutine("kickForceIncrementation");
            Invoke("handleKick", kickCooldown / 2);
            Invoke("stopKicking", kickCooldown / 2);
        }

        // 
        // Tackling
        if(Input.GetKeyDown(tackleKey) && !isTackling){
            isTackling = true;
            StartCoroutine("tackleSpeedIncrementation");
        }
        if(Input.GetKeyUp(tackleKey) && isTackling){
            movement.canMove = false;
            canShowTackle = true;
            tackleDirection = movement.lastDirection;
            StopCoroutine("tackleSpeedIncrementation");
            Invoke("stopTackling", tackleCooldown);
        }
        if(canShowTackle){
            rigidbody.AddForce(tackleDirection * currentTackleSpeed * tackleForceMultiplier, ForceMode.Force);
        }


        // 
        // Diving
        if(Input.GetKeyDown(diveKey) && !isDiving){
            isDiving = true;
            StartCoroutine("diveForceIncrementation");
        }
        if(Input.GetKeyUp(diveKey) && isDiving){
            movement.canMove = false;
            canShowDive = true;
            diveDirection = movement.lastDirection;
            StopCoroutine("diveForceIncrementation");
            Invoke("stopDiving", diveCooldown);
        }
        if(canShowDive){
            rigidbody.AddForce((diveDirection * currentDiveForce) * divingForceMultiplier, ForceMode.Force);
        }



        // 
        // Clamping values
        currentKickForce = Mathf.Clamp(currentKickForce, 0, kickForce);
        currentTackleSpeed = Mathf.Clamp(currentTackleSpeed, 0, tackleSpeed);
        currentDiveForce = Mathf.Clamp(currentDiveForce, 0, diveForce);
        currentUpForce = Mathf.Clamp(currentUpForce, 0, upForce);
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        //
        // Animator States
        animator.SetBool("isKicking", canShowKick);
        animator.SetBool("isTackling", canShowTackle);
        animator.SetBool("isDiving", canShowDive);
        animator.SetBool("isRunning", movement.direction != Vector3.zero);

        if(Input.GetKeyDown(tackledDebugKey)){
            animator.SetBool("isTackled", true);
        }else{
            animator.SetBool("isTackled", false);
        }
    }


    // 
    // Kicking
    void stopKicking(){
        isKicking = false;
        canShowKick = false;
        movement.canMove = true;
    }
    void handleKick(){
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + movement.lastDirection - Vector3.up * kickDetectionOffset, kickDetectionRange, ballLayer);
        for(int i = 0; i < colliders.Length; i++){
            ball = colliders[i].transform.gameObject.GetComponent<Rigidbody>();
            if(!isUpForce){
                ball.AddForce(this.transform.forward * currentKickForce * kickForceMultiplier, ForceMode.Impulse);
            }else{
                ball.AddForce(this.transform.forward * currentKickForce * kickForceMultiplier  + Vector3.up * currentUpForce, ForceMode.Impulse);
            }
        }

        currentKickForce = 0;
    }
    IEnumerator kickForceIncrementation(){
        while(currentKickForce < kickForce){
            currentKickForce += kickMultiplier;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }


    // 
    // Tackling
    void stopTackling(){
        canShowTackle = false;
        isTackling = false;
        movement.canMove = true;
        currentTackleSpeed = 0;
    }
    IEnumerator tackleSpeedIncrementation(){
        while(currentTackleSpeed < tackleSpeed){
            currentTackleSpeed += tackleMultiplier;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }


    // 
    // Diving
    void stopDiving(){
        canShowDive = false;
        isDiving = false;
        movement.canMove = true;
        currentDiveForce = 0;
    }
    IEnumerator diveForceIncrementation(){
        while(currentDiveForce < diveForce){
            currentDiveForce += diveMultiplier;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    //
    // Up force incrementation

    IEnumerator upForceIncrementation(){
        while(currentUpForce < upForce){
            currentUpForce += upForceMultiplier;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
    void resetUpForce(){
        currentUpForce = 0;
        isUpForce = false;
    }


    // 
    // Stamina

    IEnumerator handleStaminaDecrease(){
        while(currentStamina > 0){
            currentStamina -= staminaDecreaseMultiplier;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
    IEnumerator handleStaminaIncrease(){
        while(currentStamina < maxStamina){
            currentStamina += staminaDecreaseMultiplier;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    } 
}