using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Heat : MonoBehaviour
{
    public float heat_Total;
    public float heat_Guns;
    public float heat_Lance;
    public float heat_Shield;
    public float heat_Thrusters;
    public float heat_Rockets;

    public float heat_Total_Apparent;
    public float heat_Guns_Apparent;
    public float heat_Lance_Apparent;
    public float heat_Shield_Apparent;
    public float heat_Thrusters_Apparent;
    public float heat_Rockets_Apparent;

    public bool accessed_Guns;
    public bool accessed_Lance;
    public bool accessed_Shield;
    public bool accessed_Thruster;
    public bool accessed_Rockets;

    public bool active_Guns;
    public bool active_Lance;
    public bool active_Shield;
    public bool active_Thruster;
    public bool active_Rockets;

    public bool lockedDown_Guns;
    public bool lockedDown_Lance;
    public bool lockedDown_Shield;
    public bool lockedDown_Thurster;
    public bool lockedDown_Rockets;
}
