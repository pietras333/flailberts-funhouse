using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [Space]
    [Header("References")]
    [SerializeField] Rigidbody rigidbody;
    [Space]
    [Header("Variables")]
    [SerializeField] public bool canMove = true;
    [SerializeField] float speed = 1000f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float stopSmoothness = 5f;
    [SerializeField] float rotationSpeedMultiplier = 10f;
    [HideInInspector] float directionX; 
    [HideInInspector] float directionZ;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public Vector3 lastDirection;


    // 
    // VISUAL DEBBUGING
    // 

    void debugDirectionGizmos(){
        Gizmos.color = Color.green;
        Vector3 direction = new Vector3(transform.position.x + directionX * 2f, transform.position.y, transform.position.z + directionZ * 2f);
        Gizmos.DrawLine(transform.position, direction);
    }

    public void OnDrawGizmosSelected(){
        debugDirectionGizmos();
    }


    public void Update(){
        directionX = Input.GetAxisRaw("Horizontal");
        directionZ = Input.GetAxisRaw("Vertical");
        direction = new Vector3(directionX, 0f, directionZ).normalized;
    }

    public void FixedUpdate(){
        move();
        rotateTowardDirection();
    }

    // 
    // Movement
    // 
    
    void move(){
        if(direction == Vector3.zero || !canMove){
            rigidbody.velocity = Vector3.Slerp(rigidbody.velocity, new Vector3(0,rigidbody.velocity.y, 0), Time.fixedDeltaTime * stopSmoothness);
        }else{
            rigidbody.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.Force); 
        }
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);
    }

    // 
    // Rotation
    // 

    void rotateTowardDirection(){
        if (direction != Vector3.zero)
        {
            lastDirection = direction;
        }
        Quaternion targetRotation = Quaternion.LookRotation(lastDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeedMultiplier * Time.fixedDeltaTime);
        
    }

}