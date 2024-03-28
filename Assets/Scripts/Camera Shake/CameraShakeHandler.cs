using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraShakeHandler : MonoBehaviour
{
    [Header("Camera Shake Handler")]
    [Space]
    [Header("Configuration")]
    [Header("Attack Configuration")]
    [SerializeField, Range(0.1f, 5f)] float attackMagnitude = 3f;
    [SerializeField, Range(0.1f, 5f)] float attackRoughness = 2f;
    [SerializeField, Range(0.01f, 3f)] float attackFadeIn = 0f;
    [SerializeField, Range(0.01f, 3f)] float attackFadeOut = 0.5f;
    [Header("Move Configuration")]
    [SerializeField, Range(0.1f, 5f)] float moveMagnitude = 0.25f;
    [SerializeField, Range(0.1f, 5f)] float moveRoughness = 0.15f;
    [SerializeField, Range(0.01f, 3f)] float moveFadeIn = 1f;
    [SerializeField, Range(0.01f, 3f)] float moveFadeOut = 0.1f;

    public void ShakeOnAttack()
    {
        CameraShaker.Instance.ShakeOnce(attackMagnitude, attackRoughness, attackFadeIn, attackFadeOut);
    }

    public void ShakeOnMove()
    {
        CameraShaker.Instance.ShakeOnce(moveMagnitude, moveRoughness, moveFadeIn, moveFadeOut);
    }
}
