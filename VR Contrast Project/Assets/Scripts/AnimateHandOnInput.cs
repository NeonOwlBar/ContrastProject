using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;
    public Animator handAnimator;
    private float prevTrigger;
    private float prevGrip;

    // Update is called once per frame
    void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        if (triggerValue != prevTrigger)
        {
            handAnimator.SetFloat("Trigger", triggerValue);
            prevTrigger = triggerValue;
        }
        //Debug.Log(triggerValue);

        float gripValue = gripAnimationAction.action.ReadValue<float>();
        if (gripValue != prevGrip)
        {
            handAnimator.SetFloat("Grip", gripValue);
            prevGrip = gripValue;
        }
    }
}
