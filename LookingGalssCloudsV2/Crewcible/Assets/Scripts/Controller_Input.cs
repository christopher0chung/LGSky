using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Controller_Input : SCG_Controller {

    public PlayerMode mode;
    public Model_Input inputModel;

    //private InputDevice device0;
    //private InputDevice device1;

    //public bool connected0;
    //public bool connected1;

    private void Awake()
    {
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
    }

    void Start()
    {
        InputManager.OnDeviceDetached += OnDeviceDetached;

        priority = 0;
        Schedule(this);

        if (ServiceLocator.instance.controllerRefs.device0 != null && ServiceLocator.instance.controllerRefs.device1 != null)
            mode = PlayerMode.Coop;
        else if (ServiceLocator.instance.controllerRefs.device0 != null || ServiceLocator.instance.controllerRefs.device1 != null)
            mode = PlayerMode.Single;
        else
            mode = PlayerMode.None;
    }

	public override void ScheduledUpdate () {

        ManageDevices();

        //ManageBools();

        if (mode == PlayerMode.Single)
            ReadForSingle();
        else if (mode == PlayerMode.Coop)
            ReadForCoop();
        else
            return;
	}

    //void ManageBools()
    //{
    //    if (device0 != null)
    //        connected0 = true;
    //    else connected0 = false;

    //    if (device1 != null)
    //        connected1 = true;
    //    else connected1 = false;
    //}

    void ManageDevices()
    {
        var inputDevice = InputManager.ActiveDevice;

        if (JoinButtonWasPressedOnDevice(inputDevice))
        {
            if (ServiceLocator.instance.controllerRefs.device0 == null && ServiceLocator.instance.controllerRefs.device1 == null)
            {
                ServiceLocator.instance.controllerRefs.device0 = inputDevice;
                mode = PlayerMode.Single;
            }
            else if (ServiceLocator.instance.controllerRefs.device0 == null && ServiceLocator.instance.controllerRefs.device1 != null && ServiceLocator.instance.controllerRefs.device1 != inputDevice)
            {
                ServiceLocator.instance.controllerRefs.device0 = inputDevice;
                mode = PlayerMode.Coop;
            }
            else if (ServiceLocator.instance.controllerRefs.device0 != null && ServiceLocator.instance.controllerRefs.device1 == null && ServiceLocator.instance.controllerRefs.device0 != inputDevice)
            {
                ServiceLocator.instance.controllerRefs.device1 = inputDevice;
                mode = PlayerMode.Coop;
            }
            else if (ServiceLocator.instance.controllerRefs.device0 != null && ServiceLocator.instance.controllerRefs.device1 != null)
                return;
        }
    }

    void ReadForSingle()
    {
        InputDevice inputDevice;

        if (ServiceLocator.instance.controllerRefs.device0 != null)
            inputDevice = ServiceLocator.instance.controllerRefs.device0;
        else 
            inputDevice = ServiceLocator.instance.controllerRefs.device1;

        inputModel.L_X = inputDevice.LeftStickX;
        inputModel.L_Y = inputDevice.LeftStickY;

        inputModel.L_Mag = Mathf.Sqrt((inputModel.L_X * inputModel.L_X) + (inputModel.L_Y * inputModel.L_Y)) * 90;
        if (inputModel.L_Mag == 0)
            inputModel.L_Brg = 0;
        else
            inputModel.L_Brg = (Mathf.Atan2(inputModel.L_Y, inputModel.L_X) * Mathf.Rad2Deg + 630) % 360;

        inputModel.L_Action_OnDown = inputDevice.LeftTrigger.WasPressed;
        inputModel.L_Action_Down = inputDevice.LeftTrigger.IsPressed;
        inputModel.L_Action_OnUp = inputDevice.LeftTrigger.WasReleased;

        inputModel.L_SwapUp_OnDown = inputDevice.LeftBumper.WasPressed;
        //if (inputDevice.LeftBumper.WasPressed)
        //    Debug.Log("LB was pressed");

        inputModel.R_X = inputDevice.RightStickX;
        inputModel.R_Y = inputDevice.RightStickY;

        inputModel.R_Mag = Mathf.Sqrt((inputModel.R_X * inputModel.R_X) + (inputModel.R_Y * inputModel.R_Y)) * 90;
        if (inputModel.R_Mag == 0)
            inputModel.R_Brg = 0;
        else
            inputModel.R_Brg = (Mathf.Atan2(inputModel.R_Y, inputModel.R_X) * Mathf.Rad2Deg + 630) % 360;

        inputModel.R_Action_OnDown = inputDevice.RightTrigger.WasPressed;
        inputModel.R_Action_Down = inputDevice.RightTrigger.IsPressed;
        inputModel.R_Action_OnUp = inputDevice.RightTrigger.WasReleased;

        inputModel.R_SwapUp_OnDown = inputDevice.RightBumper.WasPressed;

        inputModel.startPause = inputDevice.CommandWasPressed;

        inputModel.acknowledge = inputDevice.Action1.WasPressed;
    }

    void ReadForCoop()
    {
        inputModel.L_X = ServiceLocator.instance.controllerRefs.device0.LeftStickX;
        inputModel.L_Y = ServiceLocator.instance.controllerRefs.device0.LeftStickY;

        inputModel.L_Mag = Mathf.Sqrt((inputModel.L_X * inputModel.L_X) + (inputModel.L_Y * inputModel.L_Y)) * 90;
        if (inputModel.L_Mag == 0)
            inputModel.L_Brg = 0;
        else
            inputModel.L_Brg = (Mathf.Atan2(inputModel.L_Y, inputModel.L_X) * Mathf.Rad2Deg + 630) % 360;

        inputModel.L_Action_OnDown = (ServiceLocator.instance.controllerRefs.device0.LeftTrigger.WasPressed || ServiceLocator.instance.controllerRefs.device0.RightTrigger.WasPressed || ServiceLocator.instance.controllerRefs.device0.Action1.WasPressed);
        inputModel.L_Action_Down = (ServiceLocator.instance.controllerRefs.device0.LeftTrigger.IsPressed || ServiceLocator.instance.controllerRefs.device0.RightTrigger.IsPressed || ServiceLocator.instance.controllerRefs.device0.Action1.IsPressed);
        inputModel.L_Action_OnUp = (ServiceLocator.instance.controllerRefs.device0.LeftTrigger.WasReleased || ServiceLocator.instance.controllerRefs.device0.RightTrigger.WasReleased || ServiceLocator.instance.controllerRefs.device0.Action1.WasReleased);

        inputModel.L_SwapUp_OnDown = ServiceLocator.instance.controllerRefs.device0.LeftBumper.WasPressed;
        inputModel.L_SwapDown_OnDown = ServiceLocator.instance.controllerRefs.device0.RightBumper.WasPressed;

        inputModel.R_X = ServiceLocator.instance.controllerRefs.device1.LeftStickX;
        inputModel.R_Y = ServiceLocator.instance.controllerRefs.device1.LeftStickY;

        inputModel.R_Mag = Mathf.Sqrt((inputModel.R_X * inputModel.R_X) + (inputModel.R_Y * inputModel.R_Y)) * 90;
        if (inputModel.R_Mag == 0)
            inputModel.R_Brg = 0;
        else
            inputModel.R_Brg = (Mathf.Atan2(inputModel.R_Y, inputModel.R_X) * Mathf.Rad2Deg + 630) % 360;

        inputModel.R_Action_OnDown = (ServiceLocator.instance.controllerRefs.device1.LeftTrigger.WasPressed || ServiceLocator.instance.controllerRefs.device1.RightTrigger.WasPressed || ServiceLocator.instance.controllerRefs.device1.Action1.WasPressed);
        inputModel.R_Action_Down = (ServiceLocator.instance.controllerRefs.device1.LeftTrigger.IsPressed || ServiceLocator.instance.controllerRefs.device1.RightTrigger.IsPressed || ServiceLocator.instance.controllerRefs.device1.Action1.IsPressed);
        inputModel.R_Action_OnUp = (ServiceLocator.instance.controllerRefs.device1.LeftTrigger.WasReleased || ServiceLocator.instance.controllerRefs.device1.RightTrigger.WasReleased || ServiceLocator.instance.controllerRefs.device1.Action1.WasReleased);

        inputModel.R_SwapUp_OnDown = ServiceLocator.instance.controllerRefs.device1.LeftBumper.WasPressed;
        inputModel.R_SwapDown_OnDown = ServiceLocator.instance.controllerRefs.device1.RightBumper.WasPressed;

        inputModel.startPause = (ServiceLocator.instance.controllerRefs.device0.CommandWasPressed || ServiceLocator.instance.controllerRefs.device1.CommandWasPressed);
        inputModel.acknowledge = (ServiceLocator.instance.controllerRefs.device0.Action1.WasPressed || ServiceLocator.instance.controllerRefs.device1.Action1.WasPressed);
    }

    bool JoinButtonWasPressedOnDevice(InputDevice inputDevice)
    {
        return inputDevice.Action1.WasPressed || inputDevice.Action2.WasPressed || inputDevice.Action3.WasPressed || inputDevice.Action4.WasPressed || inputDevice.CommandWasPressed;
    }

    void OnDeviceDetached(InputDevice inputDevice)
    {
        if (inputDevice == ServiceLocator.instance.controllerRefs.device0)
            ServiceLocator.instance.controllerRefs.device0 = null;
        if (inputDevice == ServiceLocator.instance.controllerRefs.device1)
            ServiceLocator.instance.controllerRefs.device1 = null;

        if (ServiceLocator.instance.controllerRefs.device0 == null && ServiceLocator.instance.controllerRefs.device1 == null)
            mode = PlayerMode.None;
        else if ((ServiceLocator.instance.controllerRefs.device0 == null && ServiceLocator.instance.controllerRefs.device1 != null) || (ServiceLocator.instance.controllerRefs.device0 != null && ServiceLocator.instance.controllerRefs.device1 == null))
            mode = PlayerMode.Single;
    }
}

public enum PlayerMode { None, Single, Coop }
