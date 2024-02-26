using UnityEngine;

public class TackledStateController : MonoBehaviour
{
    [Header("Tackled State Controller")]
    [Space]
    [Header("References")]

    [SerializeField] Movement movement;
    [SerializeField] Animator animator;
    [Space]
    [Header("Configuration")]
    [SerializeField] float tackledAnimationDuration = 2.3f;
    [Space]
    [Header("States")]
    [SerializeField] bool isTackled;

    void Start()
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        if (!movement || !animator)
        {
            Debug.LogError("One or more references are missing in the TackledStateController script.", gameObject);
            return;
        }
    }

    void Update()
    {
        UpdateTackledState();
    }

    void UpdateTackledState()
    {
        animator.SetBool("isTackled", isTackled);
    }

    public void Tackled()
    {
        isTackled = true;
        movement.canMove = false;
        Invoke(nameof(StopTackled), tackledAnimationDuration);
    }

    void StopTackled()
    {
        isTackled = false;
        movement.canMove = true;
    }
}