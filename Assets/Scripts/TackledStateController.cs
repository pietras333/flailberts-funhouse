using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackledStateController : MonoBehaviour
{
    [Header("Tackled State Controller")]
    [Space]
    [SerializeField] float tackledAnimationDuration = 2.3f;
    [SerializeField] bool isTackled;
    [HideInInspector] Movement movement;
    [HideInInspector] Animator animator;

    void Start(){
        animator = this.GetComponentInChildren<Animator>();
        movement = this.GetComponent<Movement>();
    }

    void Update(){
        animator.SetBool("isTackled", isTackled);
    }

    public void Tackled(){
        isTackled = true;
        movement.canMove = false;
        Invoke("stopTackled", tackledAnimationDuration);
    }
    void stopTackled(){
        isTackled = false;
        movement.canMove = true;
    }
}
