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
    [Header("Up force")]
    [SerializeField] bool isUpForce;
    [SerializeField] KeyCode upForceKey = KeyCode.LeftShift;
    [SerializeField] float upForceMultiplier = 0.45f;
    [SerializeField] float upForce = 10f;
    [SerializeField] float actualUpForce;
    [Space]
    [Header("Curving")]
    [SerializeField] bool isCurvingBall;
    [SerializeField] float curveCooldown = 1f;
    [SerializeField] KeyCode curveBallLeftKey = KeyCode.Q;
    [SerializeField] KeyCode curveBallRightKey = KeyCode.E;
    [SerializeField] Vector3 curveDirection;
    [SerializeField] float curveForceMultiplier = 1f;
    [SerializeField] float curveForce = 8f;
    [SerializeField] float actualCurveForce;

    [Space]
    [Header("Kicking")]
    [SerializeField] KeyCode kickKey = KeyCode.F;
    [SerializeField] float kickForce = 10f;
    [SerializeField] float kickCooldown = 0.25f;
    [SerializeField] float kickMultiplier = 0.65f;
    [SerializeField] float kickForceMultiplier = 2f;
    [SerializeField] float actualKickForce;
    [HideInInspector] public bool isKicking;
    [HideInInspector] bool canShowKick;
    [Space]
    [Header("Tackling")]
    [SerializeField] KeyCode tackleKey = KeyCode.G;
    [SerializeField] float tackleSpeed = 15f;
    [SerializeField] float tackleCooldown = 1f;
    [SerializeField] float tackleMultiplier = 3f;
    [SerializeField] float tackleForceMultiplier = 3f;
    [SerializeField] float actualTackleSpeed;
    [HideInInspector] public bool isTackling;
    [HideInInspector] bool canShowTackle;
    [HideInInspector] Vector3 tackleDirection;
    [Space]
    [Header("Diving")]
    [SerializeField] KeyCode diveKey = KeyCode.B;
    [SerializeField] float diveForce = 10f;
    [SerializeField] float diveCooldown = 1f;
    [SerializeField] float diveMultiplier = 1f;
    [SerializeField] float actualDiveForce;
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
        Vector3 direction = this.transform.position + movement.lastDirection - Vector3.up * 0.5f;
        Gizmos.DrawSphere(direction, 0.75f);
    }

    void debugVelocityVector(){
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward);
    }

    public void OnDrawGizmosSelected(){
        debugKickDetection();
        debugVelocityVector();
    }

    void FixedUpdate(){
        if(ball != null){
            Vector3 velocity = Vector3.Lerp(ball.velocity, Vector3.zero, 3f * Time.fixedDeltaTime);
            Vector3 direction = Vector3.Cross(velocity, Physics.gravity.normalized) * curveForce;
            ball.AddForce(direction, ForceMode.Force);
        }
    }

    void Update(){
        // 
        // Curving
        if(Input.GetKeyDown(curveBallLeftKey) || Input.GetKeyDown(curveBallRightKey) && !isCurvingBall){
            isCurvingBall = true;
            StartCoroutine("curveForceIncrementation");
        }
        if(Input.GetKeyUp(curveBallLeftKey) || Input.GetKeyUp(curveBallRightKey) && isCurvingBall){
            StopAllCoroutines();
            Invoke("resetCurveForce", curveCooldown);
        }
        if(Input.GetKey(curveBallLeftKey) && !Input.GetKey(curveBallRightKey)){
            curveDirection = -Vector3.right;
        }else if(Input.GetKey(curveBallRightKey) && !Input.GetKey(curveBallLeftKey)){
            curveDirection = Vector3.right;
        }else{
            curveDirection = Vector3.zero;
        }

        // 
        // Up-force switch
        if(Input.GetKeyDown(upForceKey) && !isUpForce){
            isUpForce = true;
            StartCoroutine("upForceIncrementation");
        }
        if(Input.GetKeyUp(upForceKey) && isUpForce){
            StopAllCoroutines();
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
            StopAllCoroutines();
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
            StopAllCoroutines();
            Invoke("stopTackling", tackleCooldown);
        }
        if(canShowTackle){
            rigidbody.AddForce(tackleDirection * actualTackleSpeed * tackleForceMultiplier, ForceMode.Force);
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
            StopAllCoroutines();
            Invoke("stopDiving", diveCooldown);
        }
        if(canShowDive){
            rigidbody.AddForce((diveDirection * actualDiveForce) * divingForceMultiplier, ForceMode.Force);
        }



        // 
        // Clamping values
        actualKickForce = Mathf.Clamp(actualKickForce, 0, kickForce);
        actualTackleSpeed = Mathf.Clamp(actualTackleSpeed, 0, tackleSpeed);
        actualDiveForce = Mathf.Clamp(actualDiveForce, 0, diveForce);
        actualUpForce = Mathf.Clamp(actualUpForce, 0, upForce);
        actualCurveForce = Mathf.Clamp(actualCurveForce, 0, curveForce);

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
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + movement.lastDirection - Vector3.up * 0.5f, 0.65f, ballLayer);
        for(int i = 0; i < colliders.Length; i++){
            ball = colliders[i].transform.gameObject.GetComponent<Rigidbody>();
            if(!isUpForce){
                ball.AddForce(this.transform.forward * actualKickForce * kickForceMultiplier, ForceMode.Impulse);
            }else{
                ball.AddForce(this.transform.forward * actualKickForce * kickForceMultiplier + Vector3.up * actualUpForce, ForceMode.Impulse);
            }
        }

        actualKickForce = 0;
    }
    IEnumerator kickForceIncrementation(){
        while(actualKickForce < kickForce){
            actualKickForce += kickMultiplier;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }


    // 
    // Tackling
    void stopTackling(){
        canShowTackle = false;
        isTackling = false;
        movement.canMove = true;
        actualTackleSpeed = 0;
    }
    IEnumerator tackleSpeedIncrementation(){
        while(actualTackleSpeed < tackleSpeed){
            actualTackleSpeed += tackleMultiplier;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }


    // 
    // Diving
    void stopDiving(){
        canShowDive = false;
        isDiving = false;
        movement.canMove = true;
        actualDiveForce = 0;
    }
    IEnumerator diveForceIncrementation(){
        while(actualDiveForce < diveForce){
            actualDiveForce += diveMultiplier;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    //
    // Up force incrementation

    IEnumerator upForceIncrementation(){
        while(actualUpForce < upForce){
            actualUpForce += upForceMultiplier;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
    void resetUpForce(){
        actualUpForce = 0;
        isUpForce = false;
    }


    //
    // curve force incrementation

    IEnumerator curveForceIncrementation(){
        while(actualCurveForce < curveForce){
            actualCurveForce += curveForceMultiplier;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
    void resetCurveForce(){
        actualCurveForce = 0;
        isCurvingBall = false;
    }
}