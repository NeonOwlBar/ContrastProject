using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class MovementInput : MonoBehaviour
{
    // reference to class for changing skybox
    public ChangeSkybox changeSky;
    // holds all left handed devices, hopefully only one
    private List<InputDevice> leftHandDevices = new();
    // determines whether player is allowed to move
    [SerializeField] private bool isMoving;
    // how far the thumbstick must be pushed in y axis to take effect
    [SerializeField] private float deadZoneY;
    public Camera cameraObj;
    public Transform cameraTransform;
    //used for setting arrow positions during development
    //bool triggerPressed = false;
    public TextMeshProUGUI coordinatesUI;
    public Image faveButtonImage;
    public List<string> faveLocations = new();


    private void OnEnable()
    {
        SearchForLeftHand();

        // subscribe respective methods to connected/disconnected events
        InputDevices.deviceConnected += RegisterConnectedDevice;
        InputDevices.deviceDisconnected += UnregisterDisconnectedDevice;
    }

    private void OnDisable()
    {
        // unsubscribe respective methods to connected/disconnected events
        InputDevices.deviceConnected += RegisterConnectedDevice;
        InputDevices.deviceDisconnected += UnregisterDisconnectedDevice;
        // empty list
        leftHandDevices.Clear();
    }

    private void SearchForLeftHand()
    {
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);

        if (leftHandDevices.Count == 1)
        {
            InputDevice device = leftHandDevices[0];
            Debug.Log("One left hand found. Device name: " + device.name);
        }
        else if (leftHandDevices.Count > 1)
        {
            Debug.LogError("Found more than one left hand!!");
        }
        else
        {
            Debug.Log("No left hand found.");
        }
    }

    private void RegisterConnectedDevice(InputDevice device)
    {
        // only looking for left handed devices, so return if already found
        if (leftHandDevices.Count > 0) return;

        SearchForLeftHand();
    }

    private void UnregisterDisconnectedDevice(InputDevice device)
    {
        if (leftHandDevices.Contains(device))
        {
            leftHandDevices.Remove(device);
        }
    }


    //private void Update()
    //{
    //    // only run if a left handed device has been found
    //    if (leftHandDevices.Count > 0)
    //    {
    //        // assigns X and Y thumbstick values to currentValue
    //        leftHandDevices[0].TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 currentValue);

    //        if (isMoving)
    //        {
    //            // allow to move again if thumbstick moved back to between positive and negative deadzones
    //            if (currentValue.y < deadZoneY && currentValue.y > -deadZoneY) 
    //                isMoving = false;
    //        }
    //        // move if y value beyond either positive or negative deadzone
    //        else if (currentValue.y > deadZoneY || currentValue.y < -deadZoneY)
    //        {
    //            //if (changeSky.levelNumber == ChangeSkybox.LevelEnum.Tutorial)
    //            //{
    //            //    changeSky.Move(currentValue.y);
    //            //}
    //            //else if (changeSky.levelNumber == ChangeSkybox.LevelEnum.One)
    //            //{
    //            //    changeSky.MoveCoordinates(currentValue.y);
    //            //}
    //            changeSky.Move(currentValue.y);
    //            isMoving = true;
    //        }

    //        //// used for setting forward and back arrow positions
    //        //leftHandDevices[0].TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerBool);
    //        //if (triggerBool)
    //        //{
    //        //    if (!triggerPressed)
    //        //    {
    //        //        //Vector3 angle = cameraTransform.eulerAngles;
    //        //        //float angleY = angle.y;
    //        //        //Debug.Log("Y rotation for forward movement for skybox " + changeSky.imgIndex + " = " + angleY);

    //        //        //Vector3 location = cameraTransform.position + transform.TransformPoint(cameraTransform.forward * 4);
    //        //        //Debug.Log("Position for forward arrow for skybox " + changeSky.imgIndex + " = " + location);
    //        //        //changeSky.tutorialForwardArrows[changeSky.imgIndex] = location;

    //        //        //triggerPressed = true;
    //        //    }
    //        //}
    //        //else
    //        //{
    //        //    if (triggerPressed) triggerPressed = false;
    //        //}
    //    }
    //}

    public void OnPressA()    // north for research, clockwise for tutorial
    {
        switch (changeSky.levelNumber)
        {
            case ChangeSkybox.LevelEnum.Tutorial:
                changeSky.ChangeSkyboxTutorial(1);
                break;
            case ChangeSkybox.LevelEnum.One:
                changeSky.ChangeSkyboxLevelOne(0, -1);
                UpdateCoordinatesText();
                break;
        }
    }
    public void OnPressB()    // east for research, anti-clockwise for tutorial
    {
        switch (changeSky.levelNumber)
        {
            case ChangeSkybox.LevelEnum.Tutorial:
                changeSky.ChangeSkyboxTutorial(-1);
                break;
            case ChangeSkybox.LevelEnum.One:
                changeSky.ChangeSkyboxLevelOne(1, 0);
                UpdateCoordinatesText();
                break;
        }
    }

    // C and D buttons only present in research level, so don't need switch statement
    public void OnPressC()    // south
    {
        changeSky.ChangeSkyboxLevelOne(0, 1);
        UpdateCoordinatesText();
    }
    public void OnPressD()    // west
    {
        changeSky.ChangeSkyboxLevelOne(-1, 0);
        UpdateCoordinatesText();
    }

    public void UpdateCoordinatesText()
    {
        string currentLoc = string.Format("{0}x{1}y", changeSky.xIndex, changeSky.yIndex);
        coordinatesUI.text = currentLoc;
        if (faveLocations.Contains(currentLoc))
        {
            faveButtonImage.color = Color.green;
        }
        else
        {
            faveButtonImage.color = Color.white;
        }
    }

    public void OnPressFave()
    {
        string currentLoc = string.Format("{0}x{1}y", changeSky.xIndex, changeSky.yIndex);
        if (!faveLocations.Contains(currentLoc))
        {
            Debug.Log($"Location {currentLoc} set as favourite");
            faveLocations.Add(currentLoc);
            faveButtonImage.color = Color.green;
        }
    }
}
