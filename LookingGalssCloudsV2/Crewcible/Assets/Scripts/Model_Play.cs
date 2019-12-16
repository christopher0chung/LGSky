using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Play : SCG_Model
{
    public PlayerState currentPlayerState;

    [Header("Stations")]
    #region Stations
    public Stations leftStation;
    public Stations rightStation;
    public List<Stations> unaccessedStations;

    public bool leftStationLocked;
    public bool rightStationLocked;
    #endregion

    [Header("Temp")]
    #region Temp Values
    public Vector4 shieldDirection;
    public float shieldSize;

    public float rocketReloadProgress;
    public float dashReloadProgress;
    #endregion

    [Header("Thrusters")]
    public float thrusterVelPerc;

    [Header("Jump")]
    public float jumpTotal;
    public float jumpRateMax;
    public float jumpOverheatPenaltyRate;

    [Header("Lives")]
    public int lives;

    [Header("General")]
    public bool isPaused;
    public float worldSpeed_Current;
    public bool deathExplosionTrigger;
    [ColorUsage(true, true)] public Color appliedWorldColor;
    [ColorUsage(true, true)] public Color appliedBulletColor;
    public float playerHP;
}
public enum PlayerState { Alive, Dead, Respawning, GameOver, LevelVictory, Dash, Flyby }