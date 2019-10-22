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
}
public enum PlayerState { Alive, Dead, Respawning, GameOver, LevelVictory }