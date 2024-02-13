using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallData : MonoBehaviour
{
    [Header("Ball Data")]
    [Space]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRange = 0.25f;
    [SerializeField] bool isGrounded;
    [SerializeField] string playerTag = "Player";
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] float ballMaxSpeed;
    [SerializeField] float inAirMaxSpeed = 15f;
    [SerializeField] float groundedMaxSpeed = 13f;
    [SerializeField] GameObject lastTouchedBy;
    
    void Update(){
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRange, groundLayer);
        if(!isGrounded){
            ballMaxSpeed = inAirMaxSpeed;
        }else{
            ballMaxSpeed = groundedMaxSpeed;
        }
    }

    public void OnCollisionEnter(Collision collider){
        if(collider.transform.gameObject != lastTouchedBy && collider.transform.gameObject.tag == playerTag){
            lastTouchedBy = collider.transform.gameObject;
        }
    }

    public void FixedUpdate(){
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, ballMaxSpeed);
    }
}
