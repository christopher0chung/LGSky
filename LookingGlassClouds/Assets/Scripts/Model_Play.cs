using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Play : SCG_Model
{
    public PlayerState currentPlayerState;
    public float pilot_flyMag;
    public float rocket_reloadProgress;
}
public enum PlayerState { Alive, Dead, Respawning, GameOver, LevelVictory }