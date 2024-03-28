using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPositionSpring : MonoBehaviour
{
    [Header("Weapon Position Spring")]
    [Space]
    [Header("Scripts")]
    [SerializeField] InputReceiver inputReceiver;
    [Header("References")]
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] Transform orientation;

    void Update()
    {
        Vector3 direction = inputReceiver.GetInputFeedback().direction;
        transform.localPosition = playerRigidbody.velocity.normalized;
    }
}