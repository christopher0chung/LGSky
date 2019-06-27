using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Energy : MonoBehaviour {

    public float jump_Actual;
    public float reactor_Actual;
    public float guns_Actual;
    public float rockets_Actual;
    public float sword_Actual;
    public float shield_Actual;
    public float pilot_Actual;

    public float jump_Apparent;
    public float reactor_Apparent;
    public float guns_Apparent;
    public float rockets_Apparent;
    public float sword_Apparent;
    public float shield_Apparent;
    public float pilot_Apparent;

    public bool gunsOn;
    public bool rocketsOn;
    public bool swordOn;
    public bool shieldOn;
    public bool pilotOn;

    public float gun_OpCost;

    public float reactor_Transient;

    public float shieldSize_Cutoff;

    public float pilot_JumpCooldownTimeRemaining;
    public float pilot_EngineOpCost;
    public float pilot_JumpOpCost;

    public float rocket_OpCost;
}
