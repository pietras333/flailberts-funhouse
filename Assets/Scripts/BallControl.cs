using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    [Header("Ball Control")]
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Movement movement;
    [SerializeField] ActionController actionController;
    [SerializeField] float ballControlForce = 0.25f;
    [SerializeField] float ballMaxSpeed = 15f;
    [HideInInspector] Rigidbody ball;
    [Space]
    [Header("Detection")]
    [SerializeField] float detectionVerticalOffset = 0.75f; 
    [SerializeField] float ballDetectionRange = 0.75f;
    [SerializeField] LayerMask ballLayer;
    void debugBallControlRange(){
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position - new Vector3(0, detectionVerticalOffset, 0), ballDetectionRange);
    }
    void OnDrawGizmosSelected(){
        debugBallControlRange();
    }

    void FixedUpdate(){
        Collider[] colliders = Physics.OverlapSphere(this.transform.position - new Vector3(0, detectionVerticalOffset, 0), ballDetectionRange, ballLayer);
        for(int i = 0; i < colliders.Length; i++){
            ball = colliders[i].transform.gameObject.GetComponent<Rigidbody>();
            if(!actionController.isKicking){
                // ball.AddForce(this.transform.forward * ballControlForce, ForceMode.Impulse);
                ball.velocity = rigidbody.velocity;
            }
            if(actionController.isTackling){
                ball.AddForce(this.transform.forward * actionController.currentTackleSpeed, ForceMode.Impulse);
            }
        }
    }

}
