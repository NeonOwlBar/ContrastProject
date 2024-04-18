using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

/// <summary>
/// Change the image for the skybox
/// </summary>
public class ChangeSkybox : MonoBehaviour
{
    [Header("General --------")]
    public LevelEnum levelNumber;
    public float totalTimer;
    public float lastMoveTime = 0f;
    public int totalMoves = 0;

    // tutorial level info
    [Header("Tutorial --------")]
    // current index in array
    public int imgIndex = 0;
    // reference to player
    public Transform playerTransform;
    public ArrowCollider arrowForward;
    public ArrowCollider arrowBackward;
    private List<ArrowCollider> arrows = new List<ArrowCollider>();
    
    public float dotProductTarget;
    [Header("UI")]
    public GameObject tutorialHandUI;
    public GameObject checkProceedUI;
    [Header("Array of Map")]
    public Material[] tutorialImages = new Material[30];
    [Header("Positions of forward arrows")]
    public Vector3[] tutorialForwardArrows = new Vector3[30];
    [Header("Positions of backward arrows")]
    public Vector3[] tutorialBackwardArrows = new Vector3[30];


    // level one info
    [Header("Level One --------")]
    [Header("UI")]
    public GameObject levelOneHandUI;
    // holds current coordinate in each axis
    [Header("Co-ordinates")]
    public int xIndex = 0;
    public int yIndex = 0;
    [Header("Array Dimensions")]
    // array dimensions
    public const int levelOneX = 6;
    public const int levelOneY = 11;
    
    [Header("Top-Down Map")] // 2D array acts as top down grid of map
    public Material[,] levelOneImages = new Material[levelOneX, levelOneY];
    [Header("Map columns")] // each one-dimensional array holds data for each column in 2D array
    public Material[] levelOneXZeroImages = new Material[levelOneY];
    public Material[] levelOneXOneImages = new Material[levelOneY];
    public Material[] levelOneXTwoImages = new Material[levelOneY];
    public Material[] levelOneXThreeImages = new Material[levelOneY];
    public Material[] levelOneXFourImages = new Material[levelOneY];
    public Material[] levelOneXFiveImages = new Material[levelOneY];
    [Header("Path indicators")]
    public GameObject pathNorth;
    public GameObject buttonNorth;
    public GameObject pathEast;
    public GameObject buttonEast;
    public GameObject pathSouth;
    public GameObject buttonSouth;
    public GameObject pathWest;
    public GameObject buttonWest;

    private List<GameObject> pathUI = new();

    public enum LevelEnum
    {
        Tutorial,
        One
    }

    // Start is called before the first frame update
    void Start()
    {
        // ensure arrays have skybox data
        PopulateLevelArrays();
        // populate lists with path indicator UI
        arrows = new List<ArrowCollider> { arrowForward, arrowBackward };
        pathUI = new List<GameObject> { pathNorth, pathEast, pathSouth, pathWest };

        // ensure start using correct skybox
        switch ((int)levelNumber)
        {
            case 0:
                SetLevelTutorial();
                break;
            case 1:
                SetLevelOne();
                break;
        }
    }

    private void Update()
    {
        switch ((int)levelNumber)
        {
            case 1:
                totalTimer += Time.deltaTime;
                break;
        }
    }

    //private void TutorialUpdate()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        ChangeSkyboxTutorial(1);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        ChangeSkyboxTutorial(-1);
    //    }
    //}

    //private void LevelOneUpdate()
    //{
    //    if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        ChangeSkyboxLevelOne(1, 0);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        ChangeSkyboxLevelOne(-1, 0);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.Y))
    //    {
    //        ChangeSkyboxLevelOne(0, 1);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.I))
    //    {
    //        ChangeSkyboxLevelOne(0, -1);
    //    }
    //}

    // load next image
    public void ChangeSkyboxTutorial(int delta)
    {
        // change skybox image index by delta (either 1 or -1)
        imgIndex += delta;
        // index wraps around as first and last are next to each other in position
        if (imgIndex < 0) imgIndex = tutorialImages.Length - 1;
        else if (imgIndex >= tutorialImages.Length) imgIndex = 0;

        RenderSettings.skybox = tutorialImages[imgIndex];

        arrowForward.transform.position = tutorialForwardArrows[imgIndex] * 50;
        Vector3 forPos = arrowForward.transform.position;
        arrowForward.transform.position = new Vector3(forPos.x, -100f, forPos.z);
        arrowBackward.transform.position = tutorialBackwardArrows[imgIndex] * 50;
        Vector3 backPos = arrowBackward.transform.position;
        arrowBackward.transform.position = new Vector3(backPos.x, -100f, backPos.z);

        PathUIFacePlayer(arrowForward.transform);
        PathUIFacePlayer(arrowBackward.transform);
    }

    private void PathUIFacePlayer(Transform UITransform)
    {
        Vector3 worldPlayerPos = transform.TransformPoint(playerTransform.position);
        // so that UI parent will look at y = -100, rotation to look upwards applied to UI as a child
        worldPlayerPos.y = -100f;
        Vector3 worldUIPos = transform.TransformPoint(UITransform.position);
        Vector3 forwardDirection = (worldUIPos - worldPlayerPos);
        Quaternion forwardRotation = Quaternion.LookRotation(forwardDirection);
        UITransform.rotation = forwardRotation;
    }

    public void Move(float joystickY)
    {
        // set direction by checking player direction. Player (Camera) is always at pos(0, 0, 0)
        int direction = 0;

        // cache player's position and direction vectors
        Vector3 playerPos = transform.TransformPoint(playerTransform.position);
        Vector3 playerForward = transform.TransformPoint(playerTransform.forward);
        //Vector3 playerVector = (playerForward - playerPos).normalized;
        //Debug.Log("vector from player to player forward = " + playerVector);
        // y value doesn't need to be compared,
        // so set y components to zero
        // all arrow position y components are zero already
        playerPos.y = 0;
        playerForward.y = 0;
        Vector3 playerVector = (playerForward - playerPos).normalized;
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

        Debug.Log("Direction = " + direction);

        // if dot product target met
        if (direction != 0)
        {
            // if player presses joystick backwards, reverse movement
            if (joystickY < 0) direction *= -1;

            ChangeSkyboxTutorial(direction);
        }
    }

    public void ChangeSkyboxLevelOne(int x, int y)
    {
        // check new values are within array bounds
        int newX = xIndex + x;
        int newY = yIndex + y;
        if (newX < 0 || newX > levelOneX - 1) return;
        if (newY < 0 || newY > levelOneY - 1) return;
        // only proceed if a skybox exists at the new coordinate
        if (levelOneImages[newX, newY] != null)
        {
            // FOR DATA COLLECTION
            // how long since last move?
            float timeSpent = totalTimer - lastMoveTime;
            // create new save data for this move
            SaveData.CreateSaveData(timeSpent, totalTimer, new Vector2Int(xIndex, yIndex), new Vector2Int(newX, newY), totalMoves);
            // update last time moved at
            lastMoveTime = totalTimer;
            // moved, so increment value
            totalMoves++;

            // FOR VISUALS
            // update coordinates
            xIndex = newX;
            yIndex = newY;
            // update skybox
            RenderSettings.skybox = levelOneImages[xIndex, yIndex];
            // set path indicator positions for new skybox
            PathPositions();
        }

        // write JSON code to file
        // * current time elapsed
        // * skybox ID or xy coordinate
        // * time spent at last scene

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
            default:
                Debug.LogError("Could not set new level");
                break;
        }
    }

    public void SetLevelTutorial()
    {
        levelNumber = LevelEnum.Tutorial;
        
        RenderSettings.skybox = tutorialImages[imgIndex];
        ChangeSkyboxTutorial(0);

        tutorialHandUI.SetActive(true);
        checkProceedUI.SetActive(false);
        levelOneHandUI.SetActive(false);

        foreach (ArrowCollider arrow in arrows) arrow.gameObject.SetActive(true);
        foreach (GameObject obj in pathUI) obj.SetActive(false);
    }

    public void SetLevelOne()
    {
        levelNumber = LevelEnum.One;
        // set skybox to use level one skybox
        RenderSettings.skybox = levelOneImages[xIndex, yIndex];
        // remove tutorial UI
        tutorialHandUI.SetActive(false);
        // activate level one UI
        levelOneHandUI.SetActive(true);
        // remove all path indicators for tutorial level
        foreach (ArrowCollider arrow in arrows) arrow.gameObject.SetActive(false);
        
        // set correct path indicators for spawn position
        PathPositions();
    }

    public void PathPositions()
    {
        // north available
        if (yIndex - 1 >= 0)
        {
            if (levelOneImages[xIndex, yIndex - 1] == null) SetNorthObjects(false);
            else SetNorthObjects(true);
        }
        else SetNorthObjects(false);

        // east available
        if (xIndex + 1 < levelOneX)
        {
            if (levelOneImages[xIndex + 1, yIndex] == null) SetEastObjects(false);
            else SetEastObjects(true);
        }
        else SetEastObjects(false);

        // south available
        if (yIndex + 1 < levelOneY)
        {
            if (levelOneImages[xIndex, yIndex + 1] == null) SetSouthObjects(false);
            else SetSouthObjects(true);
        }
        else SetSouthObjects(false);

        // west available
        if (xIndex - 1 >= 0)
        {
            if (levelOneImages[xIndex - 1, yIndex] == null) SetWestObjects(false);
            else SetWestObjects(true);
        }
        else SetWestObjects(false);




        //bool needsDiagonalToEast = (xIndex == 2 && yIndex == 4);
        //bool needsDiagonalToWest = (xIndex == 3 && yIndex == 5);

        //// north available
        //if (yIndex - 1 >= 0)
        //{
        //    if (levelOneImages[xIndex, yIndex - 1] == null) SetNorthObjects(false);
        //    else SetNorthObjects(true);
        //}
        //else SetNorthObjects(false);

        //if (needsDiagonalToEast)
        //{
        //    // east available
        //    if (xIndex + 1 < levelOneX)
        //    {
        //        if (levelOneImages[xIndex + 1, yIndex + 1] == null) SetEastObjects(false);
        //        else SetEastObjects(true);
        //    }
        //    else SetEastObjects(false);
        //}
        //else
        //{
        //    // east available
        //    if (xIndex + 1 < levelOneX)
        //    {
        //        if (levelOneImages[xIndex + 1, yIndex] == null) SetEastObjects(false);
        //        else SetEastObjects(true);
        //    }
        //    else SetEastObjects(false);
        //}
        //// south available
        //if (yIndex + 1 < levelOneY)
        //{
        //    if (levelOneImages[xIndex, yIndex + 1] == null) SetSouthObjects(false);
        //    else SetSouthObjects(true);
        //}
        //else SetSouthObjects(false);

        //if (needsDiagonalToWest)
        //{
        //    // west available
        //    if (xIndex - 1 >= 0)
        //    {
        //        if (levelOneImages[xIndex - 1, yIndex - 1] == null) SetWestObjects(false);
        //        else SetWestObjects(true);
        //    }
        //    else SetWestObjects(false);
        //}
        //else
        //{
        //    // west available
        //    if (xIndex - 1 >= 0)
        //    {
        //        if (levelOneImages[xIndex - 1, yIndex] == null) SetWestObjects(false);
        //        else SetWestObjects(true);
        //    }
        //    else SetWestObjects(false);
        //}
    }

    // set path canvas and UI buttons to true or false
    private void SetNorthObjects(bool value)
    {
        pathNorth.SetActive(value);
        buttonNorth.SetActive(value);
    }
    private void SetEastObjects(bool value)
    {
        pathEast.SetActive(value);
        buttonEast.SetActive(value);
    }
    private void SetSouthObjects(bool value)
    {
        pathSouth.SetActive(value);
        buttonSouth.SetActive(value);
    }
    private void SetWestObjects(bool value)
    {
        pathWest.SetActive(value);
        buttonWest.SetActive(value);
    }
}
