using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MovementInput : MonoBehaviour
{
    //// store left handed controller
    //List<UnityEngine.XR.InputDevice> lefthandedControllers = new();

    //// used to find the left controller
    //InputDeviceCharacteristics desiredCharacteristics = InputDeviceCharacteristics.HeldInHand
    //                                                    | InputDeviceCharacteristics.Left
    //                                                    | InputDeviceCharacteristics.Controller;

    //public InputActionProperty leftThumbstick;

    //public ChangeSkybox changeSky;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, lefthandedControllers);
    //    foreach (var device in lefthandedControllers)
    //    {
    //        Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.characteristics.ToString()));
    //    }
    //}
    //private void OnEnable()
    //{
    //    InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, lefthandedControllers);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, lefthandedControllers);
    //    //if (lefthandedControllers.Count > 0)
    //    //{
    //    //    Debug.Log("Devices found: " + lefthandedControllers.Count);
    //    //}
    //    //else
    //    //{
    //    //    Debug.Log("No devices found");
    //    //}

    //}

    // reference to class for changing skybox
    public ChangeSkybox changeSky;
    // holds all left handed devices, hopefully only one
    private List<InputDevice> leftHandDevices = new();
    // determines whether player is allowed to move
    [SerializeField] private bool isMoving;
    // how far the thumbstick must be pushed in y axis to take effect
    [SerializeField] private float deadZoneY;

    private void Start()
    {
    }

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


    private void Update()
    {
        // only run if a left handed device has been found
        if (leftHandDevices.Count > 0)
        {
            // assigns X and Y thumbstick values to currentValue
            leftHandDevices[0].TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 currentValue);

            if (isMoving)
            {
                // allow to move again if thumbstick moved back to between positive and negative deadzones
                if (currentValue.y < deadZoneY && currentValue.y > -deadZoneY) 
                    isMoving = false;
            }
            // move if y value beyond either positive or negative deadzone
            else if (currentValue.y > deadZoneY || currentValue.y < -deadZoneY)
            {
                changeSky.Move(currentValue.y);
                isMoving = true;
            }
        }
    }
}
