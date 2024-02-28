using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [Header("Bounce Pad")]
    [Space]
    [Header("Configuration")]
    [SerializeField] string playerTag = "Player";
    [SerializeField] float pushForce = 500f;


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (collision.transform.gameObject.tag == playerTag)
        {
            collision.transform.GetComponentInParent<Rigidbody>().AddForce(transform.forward * pushForce, ForceMode.Impulse);
        }
    }
}
