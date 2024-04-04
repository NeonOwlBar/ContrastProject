using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
// for volume profile components
using UnityEngine.Rendering.Universal;

public class ChangeContrast : MonoBehaviour
{
    public Volume volume;
    public float contrastOne;
    public float contrastTwo;
    public float contrastThree;
    public float contrastFour;
    public float contrastFive;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SetContrast(contrastOne);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            SetContrast(contrastTwo);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            SetContrast(contrastThree);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            SetContrast(contrastFour);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            SetContrast(contrastFive);
        }
    }

    private void SetContrast(float newValue)
    {
        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.contrast.value = newValue;
        }
    }
}
