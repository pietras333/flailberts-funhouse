using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScoring : MonoBehaviour
{
    [Header("Goal Scoring")]
    [Space]
    [SerializeField] bool isScored;
    [SerializeField] Transform ball;
    [SerializeField] Collider collider;
    [SerializeField] float ballDetectionThreshold = 1f;
    [SerializeField] float gameResetCooldown = 5f;
    [SerializeField] float ballStopSpeed = 20f;
    [SerializeField] List <ParticleSystem> confettis = new List<ParticleSystem>();

    public void OnTriggerStay(Collider foreignCollider){
        if(foreignCollider.gameObject.layer == ball.gameObject.layer){
            if(collider.bounds.Contains(foreignCollider.bounds.min * ballDetectionThreshold) && collider.bounds.Contains(foreignCollider.bounds.max * ballDetectionThreshold)){
                Rigidbody ballRigidbody = ball.transform.GetComponent<Rigidbody>();
                if(!isScored){
                    handleConfettis();
                }
                Invoke("handleGameReset", gameResetCooldown);
                ballRigidbody.velocity = Vector3.zero;
            }
        }
    }

    void handleConfettis(){
        for(int i = 0; i < confettis.Count; i++){
            confettis[i].Play();
        }
        isScored = true;
    }

    
    void handleGameReset(){
        ball.transform.position = new Vector3(0,10,0);
        isScored = false;
    }
}
