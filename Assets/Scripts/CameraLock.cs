using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    [Header("Camera Lock")]
    [Space]
    [SerializeField] Camera camera;
    [SerializeField] Transform ball;
    [SerializeField] Transform player;
    [SerializeField] float followSpeed;

    void Update(){
        Quaternion lookOnRotationBall = Quaternion.LookRotation(ball.transform.position - this.transform.position);
        Quaternion lookOnRotationPlayer = Quaternion.LookRotation(player.transform.position - this.transform.position);

        float distanceToBall = Vector3.Distance(player.transform.position, ball.transform.position);
        Quaternion crossRotation = Quaternion.Slerp(lookOnRotationPlayer, lookOnRotationBall,  0.5f);

        this.transform.position = Vector3.Slerp(this.transform.position, new Vector3(player.transform.position.x + (distanceToBall * 0.5f), this.transform.position.y, this.transform.position.z), followSpeed * Time.deltaTime);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, crossRotation, followSpeed * Time.deltaTime);
    }
}
