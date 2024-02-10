using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScoring : MonoBehaviour
{
    [Header("Goal Scoring")]
    [Space]
    [SerializeField] Transform ball;
    [SerializeField] Collider collider;
    [SerializeField] float ballDetectionThreshold = 1f;
    [SerializeField] float gameResetCooldown = 5f;
    [SerializeField] float ballStopSpeed = 20f;


    void handleGameReset(){
        ball.transform.position = new Vector3(0,10,0);
    }

    public void OnTriggerStay(Collider foreignCollider){
        if(foreignCollider.gameObject.layer == ball.gameObject.layer){
            if(collider.bounds.Contains(foreignCollider.bounds.min * ballDetectionThreshold) && collider.bounds.Contains(foreignCollider.bounds.max * ballDetectionThreshold)){
                print("Goal scored");
                Rigidbody ballRigidbody = ball.transform.GetComponent<Rigidbody>();
                float ballSpeed = ballRigidbody.velocity.magnitude;
                ballRigidbody.velocity = Vector3.Slerp(ballRigidbody.velocity, Vector3.zero, ballSpeed * ballStopSpeed * Time.deltaTime);
                Invoke("handleGameReset", gameResetCooldown);
            }
        }
    }
}
