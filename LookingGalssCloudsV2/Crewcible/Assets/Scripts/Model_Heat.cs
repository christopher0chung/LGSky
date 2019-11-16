using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Heat : SCG_Model
{
    [Header("Real Time")]
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

    public bool active_Guns;
    public bool active_Lance;
    public bool active_Shield;
    public bool active_Thrusters;
    public bool active_Rockets;

    public bool active_Dash;

    public bool overheated_Guns;
    public bool overheated_Lance;
    public bool overheated_Shield;
    public bool overheated_Thrusters;
    public bool overheated_Rockets;

    [Header("Tuning")]
    public float activationHeat_Guns;
    public float activationHeat_Lance;
    public float activationHeat_Shield;
    public float activationHeat_Thrusters;
    public float activationHeat_Rockets;

    public float sustainedHeatRate_Guns;
    public float sustainedHeatRate_Lance;
    public float sustainedHeatRate_Shield;
    public float sustainedHeatRate_Thrusters;
    public float sustainedHeatRate_Rockets;

    public float cooldownRate_Guns;
    public float cooldownRate_Lance;
    public float cooldownRate_Shield;
    public float cooldownRate_Thrusters;
    public float cooldownRate_Rockets;

    public float rebootThreshold_Guns;
    public float rebootThreshold_Lance;
    public float rebootThreshold_Shield;
    public float rebootThreshold_Thrusters;
    public float rebootThreshold_Rockets;

    public float max_HeatTotal;
}