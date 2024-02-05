using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    [Header("Camera Lock")]
    [Space]
    [SerializeField] Transform ball;
    [SerializeField] float followSpeed;

    void Update(){
        Quaternion lookOnRotation = Quaternion.LookRotation(ball.transform.position - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookOnRotation, followSpeed * Time.deltaTime);
    }
}
