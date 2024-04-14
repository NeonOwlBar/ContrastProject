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
    public const int levelOneX = 6;
    public const int levelOneY = 11;
    public Material[,] levelOneImages = new Material[levelOneX, levelOneY];
    public Material[] levelOneXZeroImages = new Material[levelOneY];
    public Material[] levelOneXOneImages = new Material[levelOneY];
    public Material[] levelOneXTwoImages = new Material[levelOneY];
    public Material[] levelOneXThreeImages = new Material[levelOneY];
    public Material[] levelOneXFourImages = new Material[levelOneY];
    public Material[] levelOneXFiveImages = new Material[levelOneY];
    public Vector3[,] levelOneLocalNorth = new Vector3[levelOneX, levelOneY];
    public int xIndex = 0;
    public int yIndex = 0;

    // level two info
    public Material[] levelTwoImages = new Material[30];


    public int imgIndex = 0;
    /*[SerializeField] */public Transform playerTransform;
    public ArrowCollider arrowForward;
    public ArrowCollider arrowBackward;
    public float dotProductTarget;

    public LevelEnum levelNumber;

    public enum LevelEnum
    {
        Tutorial,
        One,
        Two
    }

    // Start is called before the first frame update
    void Start()
    {
        // ensure arrays have skybox data
        PopulateLevelArrays();

        // ensure start using correct skybox
        switch ((int)levelNumber)
        {
            case 0: 
                RenderSettings.skybox = tutorialImages[imgIndex];
                break;
            case 1:
                RenderSettings.skybox = levelOneImages[0, 0];
                break;
        }
    }

    private void Update()
    {
        switch((int)levelNumber)
        {
            case 0:
                TutorialUpdate();
                break;
            case 1:
                LevelOneUpdate();
                break;
        }
    }

    private void TutorialUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeSkyboxTutorial(1);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeSkyboxTutorial(-1);
        }
    }

    private void LevelOneUpdate()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ChangeSkyboxLevelOne(1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeSkyboxLevelOne(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            ChangeSkyboxLevelOne(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeSkyboxLevelOne(0, -1);
        }
    }

    // load next image
    public void ChangeSkyboxTutorial(int delta)
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

            ChangeSkyboxTutorial(direction);
        }
    }

    private void ChangeSkyboxLevelOne(int x, int y)
    {
        // check new values are within array bounds
        int newX = xIndex + x;
        int newY = yIndex + y;
        if (newX < 0 || newX > levelOneX - 1) return;
        if (newY < 0 || newY > levelOneY - 1) return;
        // only proceed if a skybox exists at the new coordinate
        if (levelOneImages[newX, newY] != null)
        {
            xIndex = newX;
            yIndex = newY;
            RenderSettings.skybox = levelOneImages[newX, newY];
        }
    }

    public void MoveCoordinates(float joystickY)
    {
        
    }


    private void PopulateLevelArrays()
    {
        // create temp array to store data within nested loop below
        Material[] currentArray;
        // hold a reference to all arrays for use within nested loop below
        List<Material[]> arrays = new List<Material[]>();
        // assigning values at initialisation did not work, therefore this is used
        arrays.Add(levelOneXZeroImages);
        arrays.Add(levelOneXOneImages);
        arrays.Add(levelOneXTwoImages);
        arrays.Add(levelOneXThreeImages);
        arrays.Add(levelOneXFourImages);
        arrays.Add(levelOneXFiveImages);

        for (int i = 0; i < levelOneX; i++)
        {
            // set array to read data from
            currentArray = arrays[i];

            for (int j = 0; j < levelOneY; j++)
            {
                // assign data at coordinates
                levelOneImages[i, j] = currentArray[j];
            }
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
        levelNumber = LevelEnum.Tutorial;
    }

    public void SetLevelOne()
    {
        levelNumber = LevelEnum.One;
    }
    public void SetLevelTwo()
    {
        levelNumber = LevelEnum.Two;
    }

}
