using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Controller_Input : MonoBehaviour {

    public PlayerMode mode;
    public Model_Input inputModel;

	void Update () {

        if (mode == PlayerMode.Single)
            ReadForSingle();
        else
            ReadForCoop();
	}

    void ReadForSingle()
    {
        var inputDevice = InputManager.ActiveDevice;

        inputModel.L_X = inputDevice.LeftStickX;
        inputModel.L_Y = inputDevice.LeftStickY;

        inputModel.L_Mag = Mathf.Sqrt((inputModel.L_X * inputModel.L_X) + (inputModel.L_Y * inputModel.L_Y)) * 90;
        inputModel.L_Brg = (Mathf.Atan2(inputModel.L_Y, inputModel.L_X) * Mathf.Rad2Deg + 630) % 360;

        inputModel.L_Action_OnDown = inputDevice.LeftTrigger.WasPressed;
        inputModel.L_Action_Down = inputDevice.LeftTrigger.IsPressed;
        inputModel.L_Action_OnUp = inputDevice.LeftTrigger.WasReleased;

        inputModel.L_SwapUp_OnDown = inputDevice.LeftBumper.WasPressed;

        inputModel.R_X = inputDevice.RightStickX;
        inputModel.R_Y = inputDevice.RightStickY;

        inputModel.R_Mag = Mathf.Sqrt((inputModel.R_X * inputModel.R_X) + (inputModel.R_Y * inputModel.R_Y)) * 90;
        inputModel.R_Brg = (Mathf.Atan2(inputModel.R_Y, inputModel.R_X) * Mathf.Rad2Deg + 630) % 360;

        inputModel.R_Action_OnDown = inputDevice.RightTrigger.WasPressed;
        inputModel.R_Action_Down = inputDevice.RightTrigger.IsPressed;
        inputModel.R_Action_OnUp = inputDevice.RightTrigger.WasReleased;

        inputModel.R_SwapUp_OnDown = inputDevice.RightBumper.WasPressed;
    }

    void ReadForCoop()
    {

    }
}

public enum PlayerMode { Single, Coop }
