using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Change the image for the skybox
/// </summary>
public class ChangeSkybox : MonoBehaviour
{
    public Material[] skyboxImages = new Material[12];
    public int imgIndex = 0;
    [SerializeField] private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = skyboxImages[imgIndex];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeSkyboxMat(1);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeSkyboxMat(-1);
        }
    }

    // load next image
    public void ChangeSkyboxMat(int delta)
    {
        // change skybox image index by delta (either 1 or -1)
        imgIndex += delta;
        // index wraps around as first and last are next to each other in position
        if (imgIndex < 0) imgIndex = skyboxImages.Length - 1;
        else if (imgIndex >= skyboxImages.Length) imgIndex = 0;

        RenderSettings.skybox = skyboxImages[imgIndex];
    }

    public void MoveForward()
    {
        // set direction by checking player direction. Player (Camera) is always at pos(0, 0, 0)
        int direction = playerTransform.forward.z > 0 ? 1 : -1;

        ChangeSkyboxMat(direction);
    }
}
