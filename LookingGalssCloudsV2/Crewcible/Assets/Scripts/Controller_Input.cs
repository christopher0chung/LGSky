using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Controller_Input : SCG_Controller {

    public PlayerMode mode;
    public Model_Input inputModel;

    private InputDevice device0;
    private InputDevice device1;

    private void Awake()
    {
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
    }

    void Start()
    {
        mode = PlayerMode.None;
        InputManager.OnDeviceDetached += OnDeviceDetached;

        priority = 0;
        Schedule(this);
    }

	public override void ScheduledUpdate () {

        ManageDevices();

        if (mode == PlayerMode.Single)
            ReadForSingle();
        else if (mode == PlayerMode.Coop)
            ReadForCoop();
        else
            return;
	}

    void ManageDevices()
    {
        var inputDevice = InputManager.ActiveDevice;

        if (JoinButtonWasPressedOnDevice(inputDevice))
        {
            if (device0 == null && device1 == null)
            {
                device0 = inputDevice;
                mode = PlayerMode.Single;
            }
            else if (device0 == null && device1 != null && device1 != inputDevice)
            {
                device0 = inputDevice;
                mode = PlayerMode.Coop;
            }
            else if (device0 != null && device1 == null && device0 != inputDevice)
            {
                device1 = inputDevice;
                mode = PlayerMode.Coop;
            }
            else if (device0 != null && device1 != null)
                return;
        }
    }

    void ReadForSingle()
    {
        InputDevice inputDevice;

        if (device0 != null)
            inputDevice = device0;
        else 
            inputDevice = device1;

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
    }

    void ReadForCoop()
    {
        inputModel.L_X = device0.LeftStickX;
        inputModel.L_Y = device0.LeftStickY;

        inputModel.L_Mag = Mathf.Sqrt((inputModel.L_X * inputModel.L_X) + (inputModel.L_Y * inputModel.L_Y)) * 90;
        if (inputModel.L_Mag == 0)
            inputModel.L_Brg = 0;
        else
            inputModel.L_Brg = (Mathf.Atan2(inputModel.L_Y, inputModel.L_X) * Mathf.Rad2Deg + 630) % 360;

        inputModel.L_Action_OnDown = (device0.LeftTrigger.WasPressed || device0.RightTrigger.WasPressed || device0.Action1.WasPressed);
        inputModel.L_Action_Down = (device0.LeftTrigger.IsPressed || device0.RightTrigger.IsPressed || device0.Action1.IsPressed);
        inputModel.L_Action_OnUp = (device0.LeftTrigger.WasReleased || device0.RightTrigger.WasReleased || device0.Action1.WasReleased);

        inputModel.L_SwapUp_OnDown = device0.LeftBumper.WasPressed;
        inputModel.L_SwapDown_OnDown = device0.RightBumper.WasPressed;

        inputModel.R_X = device1.LeftStickX;
        inputModel.R_Y = device1.LeftStickY;

        inputModel.R_Mag = Mathf.Sqrt((inputModel.R_X * inputModel.R_X) + (inputModel.R_Y * inputModel.R_Y)) * 90;
        if (inputModel.R_Mag == 0)
            inputModel.R_Brg = 0;
        else;
            inputModel.R_Brg = (Mathf.Atan2(inputModel.R_Y, inputModel.R_X) * Mathf.Rad2Deg + 630) % 360;

        inputModel.R_Action_OnDown = (device1.LeftTrigger.WasPressed || device1.RightTrigger.WasPressed || device1.Action1.WasPressed);
        inputModel.R_Action_Down = (device1.LeftTrigger.IsPressed || device1.RightTrigger.IsPressed || device1.Action1.IsPressed);
        inputModel.R_Action_OnUp = (device1.LeftTrigger.WasReleased || device1.RightTrigger.WasReleased || device1.Action1.WasReleased);

        inputModel.R_SwapUp_OnDown = device1.LeftBumper.WasPressed;
        inputModel.R_SwapDown_OnDown = device1.RightBumper.WasPressed;
    }

    bool JoinButtonWasPressedOnDevice(InputDevice inputDevice)
    {
        return inputDevice.Action1.WasPressed || inputDevice.Action2.WasPressed || inputDevice.Action3.WasPressed || inputDevice.Action4.WasPressed;
    }

    void OnDeviceDetached(InputDevice inputDevice)
    {
        if (inputDevice == device0)
            device0 = null;
        if (inputDevice == device1)
            device1 = null;

        if (device0 == null && device1 == null)
            mode = PlayerMode.None;
        else if ((device0 == null && device1 != null) || (device0 != null && device1 == null))
            mode = PlayerMode.Single;
    }
}

public enum PlayerMode { None, Single, Coop }
