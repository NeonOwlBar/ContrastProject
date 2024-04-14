using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Change the image for the skybox
/// </summary>
public class ChangeSkybox : MonoBehaviour
{
    // tutorial level info
    public Material[] tutorialImages = new Material[30];
    public Vector3[] tutorialForwardArrows = new Vector3[30];
    public Vector3[] tutorialBackwardArrows = new Vector3[30];

    // level one info
    public Material[] levelOneImages = new Material[30];
    public Vector3[] levelOneForwardArrows = new Vector3[30];
    public Vector3[] levelOneBackwardArrows = new Vector3[30];

    // level two info
    public Material[] levelTwoImages = new Material[30];
    public Vector3[] levelTwoForwardArrows = new Vector3[30];
    public Vector3[] levelTwoBackwardArrows = new Vector3[30];


    public int imgIndex = 0;
    /*[SerializeField] */public Transform playerTransform;
    public ArrowCollider arrowForward;
    public ArrowCollider arrowBackward;
    public float dotProductTarget;
    public CurrentLevel currentLevel;

    public struct CurrentLevel
    {
        public string levelName;
        public Material[] skyboxImages;
        public Vector3[] forwardArrows;
        public Vector3[] backwardArrows;
    }

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = tutorialImages[imgIndex];
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
        if (imgIndex < 0) imgIndex = tutorialImages.Length - 1;
        else if (imgIndex >= tutorialImages.Length) imgIndex = 0;

        RenderSettings.skybox = tutorialImages[imgIndex];

        arrowForward.ChangePosition(tutorialForwardArrows[imgIndex]);
        arrowBackward.ChangePosition(tutorialBackwardArrows[imgIndex]);
    }

    public void Move(float joystickY)
    {
        // set direction by checking player direction. Player (Camera) is always at pos(0, 0, 0)
        int direction = 0;

        // cache player's position and direction vectors
        Vector3 playerPos = transform.TransformPoint(playerTransform.position);
        Vector3 playerForward = transform.TransformPoint(playerTransform.forward);
        Vector3 playerVector = (playerForward - playerPos).normalized;
        // y value doesn't need to be compared,
        // so set y components to zero
        // all arrow position y components are zero already
        playerPos.y = 0;
        playerForward.y = 0;
        playerVector.y = 0;
        
        // calculate normalised vectors from player to arrows
        Vector3 idealForwardVec = (tutorialForwardArrows[imgIndex] - playerPos).normalized;
        Vector3 idealBackwardVec = (tutorialBackwardArrows[imgIndex] - playerPos).normalized;
        // find dot products between vectors to arrows and forward vector
        float forwardDot = Vector3.Dot(idealForwardVec, playerVector);
        float backwardDot = Vector3.Dot(idealBackwardVec, playerVector);

        // higher dot product is the one the player is facing most
        if (forwardDot > backwardDot && forwardDot > dotProductTarget)
        {
            direction = 1;
        }
        else if (backwardDot > dotProductTarget)
        {
            direction = -1;
        }

        // if dot product target met
        if (direction != 0)
        {
            // if player presses joystick backwards, reverse movement
            if (joystickY < 0) direction *= -1;

            ChangeSkyboxMat(direction);
        }
    }

    public void SetCurrentLevel(int levelIndex)
    {
        switch (levelIndex)
        {
            case 0:
                SetLevelTutorial();
                break;
            case 1:
                SetLevelOne();
                break;
            case 2:
                SetLevelTwo();
                break;
            default:
                Debug.LogError("Could not set new level");
                break;
        }
    }

    public void SetLevelTutorial()
    {
        currentLevel.levelName = "tutorial";
        currentLevel.skyboxImages = tutorialImages;
        currentLevel.forwardArrows = tutorialForwardArrows;
        currentLevel.backwardArrows = tutorialBackwardArrows;
    }

    public void SetLevelOne()
    {
        currentLevel.levelName = "tutorial";
        currentLevel.skyboxImages = tutorialImages;
        currentLevel.forwardArrows = tutorialForwardArrows;
        currentLevel.backwardArrows = tutorialBackwardArrows;
    }
    public void SetLevelTwo()
    {

    }

}
