using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallData : MonoBehaviour
{
    [Header("Ball Data")]
    [Space]
    [SerializeField] string playerTag = "Player";
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] float ballMaxSpeed = 15f;
    [SerializeField] GameObject lastTouchedBy;
    
    public void OnCollisionEnter(Collision collider){
        if(collider.transform.gameObject != lastTouchedBy && collider.transform.gameObject.tag == playerTag){
            lastTouchedBy = collider.transform.gameObject;
        }
    }

    public void FixedUpdate(){
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, ballMaxSpeed);
    }
}
