using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;
    public GameObject pokeObject;
    public Animator handAnimator;
    //private float prevTrigger;
    //private float prevGrip;

    // used when allowing UI interaction on point animation only
    //private bool triggerChanged;
    //private bool gripChanged;

    // Update is called once per frame
    void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        //if (triggerValue != prevTrigger)
        //{
            handAnimator.SetFloat("Trigger", triggerValue);
            //prevTrigger = triggerValue;
            //triggerChanged = true;
        //}
        //Debug.Log(triggerValue);

        float gripValue = gripAnimationAction.action.ReadValue<float>();
        //if (gripValue != prevGrip)
        //{
            handAnimator.SetFloat("Grip", gripValue);
            //prevGrip = gripValue;
            //gripChanged = true;
        //}

        // two separate statements so only reset when necessary
        //if (triggerChanged || gripChanged)
        //{
        //    if (triggerValue == 0 && gripValue == 1 && !pokeObject.activeInHierarchy)
        //    {
        //        pokeObject.SetActive(true);
        //    }
        //    else if (pokeObject.activeInHierarchy)
        //    {
        //        pokeObject.SetActive(false);
        //    }

        //    // reset bools
        //    gripChanged = false;
        //    triggerChanged = false;
        //}
    }
}
