using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [Header("Combat")]
    [Space]
    [Header("References")]
    [SerializeField] Animator combatAnimator;
    [Header("Scripts")]
    [SerializeField] InputReceiver inputReceiver;
    [SerializeField] float comboTime = 1f;
    [HideInInspector] int comboIndex;
    [HideInInspector] float timeFromLastAttack = 0;

    private void Update()
    {
        if (Input.GetKeyDown(inputReceiver.GetCombatFeedback().attackKey))
        {
            if (comboIndex == 2)
            {
                comboIndex = 0;
                return;
            }
            comboIndex++;
            timeFromLastAttack = 0;
        }
        timeFromLastAttack += Time.deltaTime;
        if (timeFromLastAttack >= comboTime)
        {
            comboIndex = 0;
        }
        combatAnimator.SetInteger("ComboIndex", comboIndex);
    }

    void ResetComboIndex()
    {
        comboIndex = 0;
    }
}
