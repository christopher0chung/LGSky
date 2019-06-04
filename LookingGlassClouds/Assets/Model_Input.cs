using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Input : MonoBehaviour {

    [Range(-1f, 1f)] public float station1_Axis_X;
    [Range(-1f, 1f)] public float station1_Axis_Y;

    public bool station1_Action_OnPress;
    public bool station1_Action_IsPressed;
    public bool station1_Action_OnRelease;

    public bool station1_Cancel_OnPress;
    public bool station1_Cancel_IsPressed;
    public bool station1_Cancel_OnRelease;

    public bool station1_SwitchUp_OnPress;
    public bool station1_SwitchDown_OnPress;

    [Range(-1f, 1f)] public float station2_Axis_X;
    [Range(-1f, 1f)] public float station2_Axis_Y;

    public bool station2_Action_OnPress;
    public bool station2_Action_IsPressed;
    public bool station2_Action_OnRelease;

    public bool station2_Cancel_OnPress;
    public bool station2_Cancel_IsPressed;
    public bool station2_Cancel_OnRelease;

    public bool station2_SwitchUp_OnPress;
    public bool station2_SwitchDown_OnPress;
}
