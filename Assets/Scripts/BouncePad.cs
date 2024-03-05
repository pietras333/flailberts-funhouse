using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [Header("Bounce Pad")]
    [Space]
    [Header("Configuration")]
    [SerializeField] LayerMask playerParentLayer;
    [SerializeField] float pushForce = 1000f;
    [SerializeField] Vector3 colliderExtents;
    [HideInInspector] Collider[] colliders;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, colliderExtents * 2f);
    }

    void FixedUpdate()
    {
        colliders = Physics.OverlapBox(transform.position, colliderExtents, transform.rotation, playerParentLayer);
        if (colliders.Length > 0)
        {
            GameObject player = colliders[^1].gameObject;
            Debug.Log(player);
            player.GetComponent<Rigidbody>().AddForce(player.transform.up * pushForce, ForceMode.Force);
        }
    }
}
