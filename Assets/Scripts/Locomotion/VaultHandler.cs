using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultHandler : MonoBehaviour
{
    [Header("Vault Handler")]
    [Space]
    [Header("Scripts")]
    [SerializeField] LocomotionParameters locomotionParameters;
    [SerializeField] GroundDetector groundDetector;
    [SerializeField] InputReceiver inputReceiver;

    [Header("References")]
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] Transform vaultDetector;
    [SerializeField] Transform orientation;

    [Header("Configuration")]
    [SerializeField, Range(0.1f, 3f)] float vaultDetectionRange = 2f;
    [SerializeField] LayerMask groundLayer;
    [HideInInspector] bool ledgeDetected;

    void Update()
    {
        ledgeDetected = DetectLedge();
        if (Input.GetKeyDown(inputReceiver.GetInputParametersFeedback().jumpKey))
        {
            HandleVault();
        }
    }

    public bool DetectLedge()
    {
        return Physics.Raycast(vaultDetector.transform.position, transform.TransformDirection(orientation.transform.forward), vaultDetectionRange, groundLayer);
    }

    void HandleVault()
    {
        if (!ledgeDetected || groundDetector.GetGroundFeedback().isGrounded || Physics.Raycast(transform.position + Vector3.up * 0.5f, orientation.forward, 2f, groundLayer))
        {
            return;
        }
        Debug.Log("Vaulted");
        // playerRigidbody.AddForce(transform.up * locomotionParameters.GetVaultParameters().vaultForce * 2, ForceMode.Impulse);
        playerRigidbody.MovePosition(transform.position + Vector3.up + orientation.forward);
    }
}
