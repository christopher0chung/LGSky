using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Game : SCG_Model {

    [Header("Global")]
    #region Global
    public float worldSpeed_fwd;
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
    #endregion
    [Header("Shield")]
    #region Shield
    //public bool shieldOn;
    public Color c_Shield_Inside;
    public Color c_Shield_Outside;
    public float f_Shield_HDRIntensity_Base;
    public float f_Shield_HDRIntensity_Boost;
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
    public float t_Guns_TimeBetweenShots;
    public float d_Guns_Damage;
    public float s_Guns_TurnSpeed_Activated;
    public float s_Guns_TurnSpeed_Inactivated;
    public float f_Guns_BulletDispersion;
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
    public float f_Rockets_Spread;
    public float t_Rockets_Lifetime;
    #endregion
    [Header("Thrusters")]
    #region Pilot
    public float s_Thrusters_Speed;
    public float f_Thrusters_DashDistance;
    public float t_Thrusters_DashCooldown;
    #endregion
    [Header("Lance")]
    #region Sword
    public float d_Lance_Damage;
    public float f_Lance_MinRange;
    public float f_Lance_MaxRange;
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
    #endregion
}

public enum Stations { Thrusters, Guns, Rockets, Shield, Lance }

