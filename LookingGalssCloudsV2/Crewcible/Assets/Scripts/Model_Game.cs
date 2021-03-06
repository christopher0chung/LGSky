﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Game : SCG_Model {

    [Header("Global")]
    #region Global
    public float worldSpeed_min;
    public float worldSpeed_max;
    public Color c_Cool;
    public Color c_Warm;
    public Color c_Hot;
    public Color c_UI_Base;
    #endregion
    [Header("Ship SFX")]
    #region Ship
    public AudioClip sfx_ShipExplode;
    public AudioClip sfx_Respawn;
    public AudioClip sfx_Thrusters_Jets;
    public AudioClip sfx_Thrusters_Dash;
    public AudioClip sfx_Shield_Block;
    public AudioClip sfx_Shield_Activate;
    public AudioClip sfx_Gun_Shot;
    public AudioClip sfx_ShipHit;
    public AudioClip sfx_LanceHit;
    public AudioClip sfx_LanceLit;
    public AudioClip sfx_RocketLaunch;
    #endregion
    [Header("Enemy SFX")]
    #region Enemy
    public AudioClip sfx_EnemyLittleExplosion;
    public AudioClip sfx_EnemyBulletHit;
    #endregion
    [Header("Shield")]
    #region Shield
    //public bool shieldOn;
    [ColorUsage(true, true)] public Color c_Shield_Inside;
    [ColorUsage(true, true)] public Color c_Shield_Outside;
    public float f_Shield_HDRIntensity_Base;
    public float f_Shield_HDRIntensity_Boost;
    public float f_Shield_ActiveBlockPenalty;
    public float f_Shield_Cutoff_Min;
    public float f_Shield_Cutoff_Max;
    //public Vector4 shieldForwardDirection;
    public float s_Shield_Grow;
    public float s_Shield_Contract;
    public float s_Shield_HitFlashNormalize;
    #endregion
    [Header("Gun")]
    #region Gun
    public float t_Guns_SpinUpTime;
    public float t_Guns_TimeBetweenShots_Max;
    public float t_Guns_TimeBetweenShots_Min;
    public float d_Guns_Damage;
    public float s_Guns_TurnSpeed_Activated;
    public float s_Guns_TurnSpeed_Inactivated;
    public float f_Guns_BulletDispersion_Max;
    public float f_Guns_BulletDispersion_Min;
    public float t_Guns_BulletLifetime;
    public float s_Guns_BulletSpeed;
    #endregion
    [Header("Rockets")]
    #region Rockets
    public float t_Rockets_FireRate;
    public float t_Rockets_Reload;
    public float d_Rockets_Damage;
    public float d_Rockets_ExplosionBallDamage;
    public float f_Rockets_ExplosionBallSize;
    public float t_Rockets_ExplosionBallLifetime;
    public int i_Rockets_RocketCountMax;
    public float s_Rockets_FlySpeed;
    public float s_Rockets_TurnRate;
    public float t_Rockets_Lifetime;
    #endregion
    [Header("Thrusters")]
    #region Pilot
    public float s_Thrusters_Speed;
    public float s_Thrusters_Accel;
    public float f_Thrusters_DashDistance;
    public float t_Thrusters_DashCooldown;
    public float f_xBoundClose;
    public float f_xBoundFar;
    public float f_zBoundClose;
    public float f_zBoundFar;
    #endregion
    [Header("Lance")]
    #region Sword
    public float d_Lance_Damage;
    public float d_Lance_Damage_Sustained;
    public float f_Lance_MinRange;
    public float f_Lance_MaxRange;
    public float f_Lance_OvermaxRange;
    public float s_Lance_ExtendSpeed;
    #endregion
    [Header("Reactor")]
    #region Reactor
    public float e_Reactor_Base;
    public float e_Reactor_Upgrade;
    public float e_ShutdownThreshold;
    public float e_ShutdownThreshold_Upgrade;
    public float e_ChargeJumpThreshold;
    public float t_shutDownTime;
    #endregion
    [Header("JumpDrive")]
    #region JumpDrive
    public float e_JumpActivateThreshold;
    #endregion
    [Header("Enemy Stats")]
    #region Enemy Stats
    public float m_EnemyBulletDamage;
    public float m_LockingBaddyPotshotTime;
    public float hp_SwarmBoy;
    public float hp_RingDude;
    public float hp_Mine;
    public float hp_Missile;
    #endregion
    [Header("Enemy Object Refs")]
    #region Enemy Object Refs
    public GameObject swarmBoyPrefab;
    public GameObject ringDudePrefab;
    public GameObject minePrefab;
    public GameObject missilePrefab;
    #endregion
    [Header("Asset Object Refs")]
    #region Asset Object Refs
    public GameObject bulletPrefab;
    public GameObject bulletExplosionPrefab;
    public GameObject rocketPrefab;
    public GameObject rocketExplosionPrefab;
    public GameObject deathExplosionPrefab;
    public GameObject mineExplosionPrefab;
    public GameObject missileExplosionPrefab;
    public GameObject sfxPlayerPrefab;
    public GameObject lanceHitPrefab;
    #endregion
    [Header("Dialog")]
    #region Dialog
    public AudioClip Welcome;
    public AudioClip Warning_Guns;
    public AudioClip Warning_Lance;
    public AudioClip Warning_Rockets;
    public AudioClip Warning_Shields;
    public AudioClip Warning_Thrusters;
    public AudioClip Warning_System;
    public AudioClip[] Online_Guns;
    public AudioClip[] Online_Lance;
    public AudioClip[] Online_Rockets;
    public AudioClip[] Online_Shields;
    public AudioClip[] Online_Thrusters;
    public AudioClip[] LD_Port;
    public AudioClip[] LD_Stbd;
    public AudioClip Danger_JC;
    public AudioClip[] CriticalError;
    public AudioClip Contact_General;
    public AudioClip Contact_Rear;
    #endregion
}

public enum Stations { Thrusters, Guns, Rockets, Shield, Lance }

