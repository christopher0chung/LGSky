using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Game : MonoBehaviour {

    [Header("Global")]
    #region Global
    public float worldSpeed_fwd;
    #endregion
    [Header("Ship")]
    #region Ship
    public AudioClip shipExplodeSound;
    public AudioClip respawnSound;
    public AudioClip pilotBoost;
    public AudioClip pilotThruster;
    #endregion
    [Header("Shield")]
    #region Shield
    //public bool shieldOn;
    public Color insideColor_Base;
    public Color outsideColor_Base;
    public float hitHDRIntensity_Base;
    public float hitHDRIntensity_Boost;
    public float cutoff_Base;
    public float cutoff_Base_Upgrade;
    public float cutoff_Boost;
    public float cutoff_Boost_Upgrade;
    //public float cutoff_Current;
    public Vector4 shieldForwardDirection;
    public float contractAndDialationRate;
    public float contractAndDialationRate_Upgrade;
    public float colorCoolDownLerpFraction;
    public float e_Shield_Passive;
    public float e_Shield_Active;
    public float e_BlockedShotEnergyHit_Base;
    public float e_BlockedShotEnergyHit_Boost;
    public AudioClip blockedShot_Base;
    public AudioClip blockedShot_Boost;
    #endregion
    [Header("Gun")]
    #region Gun
    //public bool gunOn;
    public float t_SpinUpTime;
    public float t_SpinUpTime_Upgrade;
    public float t_TimeBetweenShots;
    public float t_TimeBetweenShots_Upgrade;
    public Color gunTurretReticleColor;
    public float gunFiringReticleHDRBoost;
    public Color bulletColor;
    public AudioClip gunShot;
    public float e_Gun_Passive;
    public float e_Gun_Active;
    public float e_Gun_CooldownRate;
    public float d_GunDamage_Base;
    public float d_GunDamage_Upgrade;
    #endregion
    [Header("Rockets")]
    #region Rockets
    //public bool rocketsOn;
    public float e_Rockets_Passive;
    public float e_Rockets_Active;
    public float e_RocketCooldownRate;
    public float t_RocketCooldown;
    public float d_ExplosionBallDamage;
    public float d_ExplosionBallSize_Base;
    public float d_ExplosionBallSize_Upgrade;
    public int rocketCountMax;
    public float t_RocketTurnTimeNormalized;
    #endregion
    [Header("Pilot")]
    #region Pilot
    //public bool pilotOn;
    public float flySpeed;
    public float boostDist;
    public float e_Pilot_Passive;
    public float e_Flying;
    public float e_Flying_UpgradeDiscount;
    public float e_Pilot_Boost;
    public float e_FlyingCooldownRate;
    public float e_BoostCooldownRate;
    public float t_BoostCooldown;
    #endregion
    [Header("Sword")]
    #region Sword
    //public bool swordOn;
    public float someRepairFloat;
    public float e_SwordEnergyRate_Passive;
    public float e_SwordEnergyRate_Active;
    public float e_SwordEnergyRate_Active_UpgradeDiscount;
    public float d_SwordDamage;
    public float d_SwordRange_Base;
    public float d_SwordRange_Upgrade;
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
    [Header("Stations")]
    #region Stations
    public Stations leftStation;
    public Stations rightStation;
    #endregion
    [Header("Enemy Stats")]
    #region Enemy Stats
    public float enemyBulletDamage;
    #endregion
    [Header("Enemy Object Refs")]
    #region Enemy Object Refs
    public GameObject swarmBoyPrefab;
    public GameObject anotherPrefab;
    #endregion
    [Header("Asset Object Refs")]
    #region Asset Object Refs
    public GameObject bulletPrefab;
    public GameObject bulletExplosionPrefab;
    public GameObject rocketPrefab;
    public GameObject rocketExplosionPrefab;
    #endregion
}

public enum Stations { Pilot, Guns, Rockets, Shield, Sword }

