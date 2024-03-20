using System.Collections;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class ViewParameters : MonoBehaviour
{
    [Header("View Parameters")]
    [Space]
    [Header("Configuration")]
    [SerializeField, Range(0.1f, 15)] float viewSensitivity = 5f; // Sensitivity of view movement
    [SerializeField, Range(50, 90)] float lockRotationX = 60f; // Maximum rotation on the X-axis
    [SerializeField, Range(0, 25f)] float cameraTiltZ = 15f; // Amount of camera tilt on the Z-axis
    [SerializeField, Range(1, 15f)] float tiltSmothness = 7f; // Smoothing factor for camera tilt
    [SerializeField, Range(0.01f, 10)] public float followSmothness = 0.05f;
    [SerializeField] public bool lockCursor; // Whether to lock the cursor

    [Header("View Shock Absorbtion")]
    [SerializeField, Range(5, 10)] float absorbtionSmothness = 7f; // Smoothing factor for shock absorption
    [SerializeField, Range(0.1f, 0.5f)] float amplitude = 0.15f; // Amplitude of shock absorption
    [SerializeField, Range(0.01f, 0.3f)] float duration = 0.05f; // Duration of shock absorption

    // Function to get rotation parameters
    public RotationParameters GetRotationParameters()
    {
        return new RotationParameters(viewSensitivity, lockRotationX, cameraTiltZ, tiltSmothness);
    }

    // Function to get shock absorption parameters
    public ShockAbsorbtionParameters GetShockAbsorbtionParameters()
    {
        return new ShockAbsorbtionParameters(absorbtionSmothness, amplitude, duration);
    }
}

// Class to hold rotation parameters
public class RotationParameters
{
    public float sensitivity; // Sensitivity of view movement
    public float lockRotationX; // Maximum rotation on the X-axis
    public float cameraTiltZ; // Amount of camera tilt on the Z-axis
    public float tiltSmothness; // Smoothing factor for camera tilt
    public RotationParameters(float sensitivity, float lockRotationX, float cameraTiltZ, float tiltSmothness)
    {
        this.sensitivity = sensitivity;
        this.lockRotationX = lockRotationX;
        this.cameraTiltZ = cameraTiltZ;
        this.tiltSmothness = tiltSmothness;
    }
}

// Class to hold shock absorption parameters
public class ShockAbsorbtionParameters
{
    public float absorbtionSmothness; // Smoothing factor for shock absorption
    public float amplitude; // Amplitude of shock absorption
    public float duration; // Duration of shock absorption

    public ShockAbsorbtionParameters(float absorbtionSmothness, float amplitude, float duration)
    {
        this.absorbtionSmothness = absorbtionSmothness;
        this.amplitude = amplitude;
        this.duration = duration;
    }
}

