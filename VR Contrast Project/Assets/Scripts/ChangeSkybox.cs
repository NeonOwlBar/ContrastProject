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
    /*[SerializeField] */public Transform playerTransform;
    public Vector3[] forwardArrowPositions = new Vector3[30];
    public Vector3[] backwardArrowPositions = new Vector3[30];
    public ArrowCollider arrowForward;
    public ArrowCollider arrowBackward;

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

        arrowForward.ChangePosition(forwardArrowPositions[imgIndex]);
        arrowBackward.ChangePosition(backwardArrowPositions[imgIndex]);
    }

    public void Move(float joystickY)
    {
        // set direction by checking player direction. Player (Camera) is always at pos(0, 0, 0)
        int direction = 0;

        // cache player's position
        Vector3 playerPos = playerTransform.position;
        // y value doesn't need to be compared,
        // and all arrow position y components are zero,
        // so set y components to zero
        playerPos.y = 0;
        Vector3 playerForward = playerTransform.forward;
        playerForward.y = 0;
        Vector3 playerVector = (playerForward - playerPos).normalized;
        playerVector.y = 0;
        
        Vector3 idealForwardVec = (forwardArrowPositions[imgIndex] - playerPos).normalized;
        Vector3 idealBackwardVec = (backwardArrowPositions[imgIndex] - playerPos).normalized;
        float forwardDot = Vector3.Dot(idealForwardVec, playerVector);
        float backwardDot = Vector3.Dot(idealBackwardVec, playerVector);
        Debug.Log("Forward dot product = " + forwardDot);
        Debug.Log("Backward dot product = " + backwardDot);
        // higher dot product is the one the player is facing most
        if (forwardDot > backwardDot && forwardDot > 0.85f)
        {
            direction = 1;
        }
        else if (backwardDot > 0.95f)
        {
            direction = -1;
        }



        //int directionMultiplier = joystickY > 0 ? 1 : -1;
        // vector to ideal position to look at
        //Vector3 idealVector = forwardArrowPositions[imgIndex] - playerPos;
        //if (Vector3.Dot(idealVector, playerVector) > 0.8f)
        //{
        //    direction = 1;
        //}
        //else
        //{
        //    idealVector = backwardArrowPositions[imgIndex] - playerPos;
        //    if (Vector3.Dot(idealVector, playerVector) > 0.8f)
        //    {
        //        direction = -1;
        //    }
        //}



        if (direction != 0)
        {
            // if player presses joystick backwards, reverse movement
            if (joystickY < 0) direction *= -1;

            ChangeSkyboxMat(direction);
        }
    }
}
