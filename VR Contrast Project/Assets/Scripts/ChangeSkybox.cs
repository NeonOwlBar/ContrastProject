using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Change the image for the skybox
/// </summary>
public class SkyboxChange : MonoBehaviour
{
    Material skyboxMat;
    //Skybox skybox;
    public Material[] skyboxImages = new Material[12];
    public int imgIndex = 0;
    private int directionInt = 0;
    private bool isKeyDown;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = skyboxImages[imgIndex];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            directionInt = 1;
            isKeyDown = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            directionInt = -1;
            isKeyDown = true;
        }
        if (isKeyDown)
        {
            ChangeSkyboxMat(directionInt);
            isKeyDown = false;
        }
        
    }

    // load next image
    public void ChangeSkyboxMat(int delta)
    {
        // change skybox image index by delta (either 1 or -1)
        imgIndex += delta;
        // keep imgIndex within bounds of skyboxImages array
        imgIndex = Math.Clamp(imgIndex, 0, skyboxImages.Length - 1);
        RenderSettings.skybox = skyboxImages[imgIndex];
    }
}
