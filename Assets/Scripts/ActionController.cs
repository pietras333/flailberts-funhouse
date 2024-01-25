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
    [Space]
    [Header("Keycodes")]
    [SerializeField] KeyCode kickKey = KeyCode.F;
    [SerializeField] float kickForce = 10f;
    [SerializeField] float kickCooldown = 0.25f;
    [SerializeField] float kickMultiplier = 0.65f;
    [SerializeField] float actualKickForce;
    [HideInInspector] public bool isKicking;
    [HideInInspector] bool canShowKick;
    [Space]
    [SerializeField] KeyCode tackleKey = KeyCode.G;
    [SerializeField] float tackleSpeed = 15f;
    [SerializeField] float tackleCooldown = 1f;
    [SerializeField] float tackleMultiplier = 3f;
    [SerializeField] float actualTackleSpeed;
    [HideInInspector] public bool isTackling;
    [HideInInspector] bool canShowTackle;
    [HideInInspector] Vector3 tackleDirection;
    [Space]
    [SerializeField] KeyCode diveKey = KeyCode.B;
    [SerializeField] float diveForce = 10f;
    [SerializeField] float diveCooldown = 1f;
    [SerializeField] float diveMultiplier = 1f;
    [SerializeField] float actualDiveForce;
    [HideInInspector] public bool isDiving;
    [HideInInspector] bool canShowDive;
    [HideInInspector] Vector3 diveDirection;
    [Space]
    [SerializeField] KeyCode tackledDebugKey = KeyCode.L;

    // 
    // VISUAL DEBUGGING
    // 

    void debugKickGizmos(){
        Gizmos.color = Color.blue;
        Vector3 direction = this.transform.position + movement.lastDirection - Vector3.up * 0.5f;
        Gizmos.DrawSphere(direction, 0.75f);
    }

    public void OnDrawGizmosSelected(){
        debugKickGizmos();
    }


    void Update(){

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
            rigidbody.AddForce(tackleDirection * actualTackleSpeed * 3f, ForceMode.Force);
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
            rigidbody.AddForce((diveDirection * actualDiveForce + Vector3.up * actualDiveForce / 5 )* 5f, ForceMode.Force);
        }



        // 
        // Clamping values
        actualKickForce = Mathf.Clamp(actualKickForce, 0, kickForce);
        actualTackleSpeed = Mathf.Clamp(actualTackleSpeed, 0, tackleSpeed);
        actualDiveForce = Mathf.Clamp(actualDiveForce, 0, diveForce);

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
            Rigidbody ball = colliders[i].transform.gameObject.GetComponent<Rigidbody>();
            ball.AddForce(this.transform.forward * actualKickForce * 2f + Vector3.up * actualKickForce, ForceMode.Impulse);
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
}