using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Game : MonoBehaviour {

    [Header("Global")]
    public float worldSpeed_fwd;
    [Header("Shield")]
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
    [Header("Gun")]
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
    public float d_GunDamage_Base;
    public float d_GunDamage_Upgrade;
    [Header("Rockets")]
    //public bool rocketsOn;
    public float e_Rockets_Passive;
    public float e_Rockets_Active;
    public float t_RocketCooldown;
    public float d_ExplosionBallDamage;
    public float d_ExplosionBallSize_Base;
    public float d_ExplosionBallSize_Upgrade;
    public int rocketCountMax;
    public float t_RocketTurnTimeNormalized;
    [Header("Pilot")]
    //public bool pilotOn;
    public float flySpeed;
    public float boostDist;
    public float e_Pilot_Passive;
    public float e_Flying;
    public float e_Flying_UpgradeDiscount;
    public float e_Pilot_Boost;
    public float t_BoostCooldown;
    [Header("Sword")]
    //public bool swordOn;
    public float someRepairFloat;
    public float e_SwordEnergyRate_Passive;
    public float e_SwordEnergyRate_Active;
    public float e_SwordEnergyRate_Active_UpgradeDiscount;
    public float d_SwordDamage;
    public float d_SwordRange_Base;
    public float d_SwordRange_Upgrade;
    [Header("Reactor")]
    public float e_Reactor_Base;
    public float e_Reactor_Upgrade;
    public float e_ShutdownThreshold;
    public float e_ShutdownThreshold_Upgrade;
    public float e_ChargeJumpThreshold;
    public float t_shutDownTime;
    [Header("JumpDrive")]
    public float e_JumpActivateThreshold;
    [Header("Stations")]
    public Stations leftStation;
    public Stations rightStation;
    [Header("Enemies")]
    public GameObject swarmBoyPrefab;
    public GameObject anotherPrefab;
    [Header("Assets")]
    public GameObject bulletPrefab;
    public GameObject bulletExplosionPrefab;
    public GameObject rocketPrefab;
    public GameObject rocketExplosionPrefab;
}

public enum Stations { Pilot, Guns, Rockets, Shield, Sword }

