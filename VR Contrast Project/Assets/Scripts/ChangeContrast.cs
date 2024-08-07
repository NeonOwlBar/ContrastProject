using UnityEngine;
using UnityEngine.Rendering;
// for volume profile components
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ChangeContrast : MonoBehaviour
{
    public Scrollbar scrollbar;
    public Scrollbar LevelOneScrollbar;

    public Volume volume;
    // preset contrast levels from lowest to highest
    public float contrastOne;   // -75
    public float contrastTwo;   // -40
    public float contrastThree; // 0
    public float contrastFour;  // 40
    public float contrastFive;  // 75

    public void SetContrast()
    {
        float contrastValue;
        if (scrollbar.isActiveAndEnabled)
        {
            // get current scrollbar value
            contrastValue = scrollbar.value;
        }
        else
        {
            // get current scrollbar value
            contrastValue = LevelOneScrollbar.value;
        }

        // checking range in case of error in scroll bar
        // for value of 0
        if (contrastValue < 0.25f)
        {
            contrastValue = contrastOne;
        }
        // for value of 0.25
        else if (contrastValue < 0.5f)
        {
            contrastValue = contrastTwo;
        }
        // for value of 0.5
        else if (contrastValue < 0.75f)
        {
            contrastValue = contrastThree;
        }
        // for value of 0.75
        else if (contrastValue < 1f)
        {
            contrastValue = contrastFour;
        }
        // for value of 1
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
