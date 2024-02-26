using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLimbMotion : MonoBehaviour
{   
    [Header("Copy Limb Motion")]
    [Space]
    [Header("References")]
    [Space]
    [SerializeField] Transform targetLimb;
    [SerializeField] bool isMirror;
    [HideInInspector] ConfigurableJoint configurableJoint;

    void Start(){
        configurableJoint = GetComponent<ConfigurableJoint>();
    }

    void LateUpdate(){
        if(!isMirror){
            configurableJoint.targetRotation = targetLimb.localRotation;
        }else{
            configurableJoint.targetRotation = Quaternion.Inverse(targetLimb.localRotation);
        }
    }
}
