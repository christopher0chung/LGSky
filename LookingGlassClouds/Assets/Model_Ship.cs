using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Ship : MonoBehaviour {

    #region Ship General
    [Header("General")]
    public Transform ship;
    public AudioSource shipAS;
    #endregion

    #region Pilot
    [Header("Pilot - Set")]
    public float moveVel;

    public float xBoundClose;
    public float xBoundFar;
    public float zBoundClose;
    public float zBoundFar;

    [Header("---[Leave]--- Pilot - Calculated")]
    public Vector3 leftBoundVector;
    public Vector3 rightBoundVector;
    public Vector3 leftInVector;
    public Vector3 rightInVector;
    #endregion

    #region Shield
    [Header("Shield - Set")]
    public Material shieldMat;
    public Vector4 shield_FwdVector;
    public float shield_CoverageDotProductThreshold;
    #endregion

    #region Guns
    [Header("Guns - Set")]
    public Transform gun_Turret;
    public Transform gun_Elevator;
    public Transform gun_Object;

    public AudioClip gun_ShotSFX;
    public float gun_FireTimeInterval;
    public float gun_FireFirstShotDelay;

    [Header("---[Leave]--- Guns - Calculated")]
    public float gun_RelBrg;
    public float gun_Elevation;

    public float gun_ShootTimer;
    #endregion

    #region Sword
    [Header("Sword - Set")]
    public Transform sword_Turret;
    public Transform sword_Elevator;
    public Transform sword_Object;

    [Header("---[Leave]--- Sword - Calculated")]
    public float sword_RelBrg;
    public float sword_Elevation;
    #endregion

    #region Rockets
    [Header("Rockets - Set")]
    private Transform rocket_Turret;
    private Transform rocket_Elevator;

    [Header("---[Leave]--- Rockets - Calculated")]
    public float rocket_RelBrg;
    public float rocket_Elevation;
    #endregion
}
