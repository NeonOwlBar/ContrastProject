using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
// for volume profile components
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ChangeContrast : MonoBehaviour
{
    public Scrollbar scrollbar;

    public Volume volume;
    // preset contrast levels from lowest to highest
    public float contrastOne;   // -75
    public float contrastTwo;   // -40
    public float contrastThree; // 0
    public float contrastFour;  // 40
    public float contrastFive;  // 75

    public void SetContrast()
    {
        // get current scrollbar value
        float contrastValue = scrollbar.value;

        if (contrastValue < 0.25f)
        {
            contrastValue = contrastOne;
        }
        else if (contrastValue < 0.5f)
        {
            contrastValue = contrastTwo;
        }
        else if (contrastValue < 0.75f)
        {
            contrastValue = contrastThree;
        }
        else if (contrastValue < 1f)
        {
            contrastValue = contrastFour;
        }
        else
        {
            contrastValue = contrastFive;
        }

        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.contrast.value = contrastValue;
        }
    }
}
