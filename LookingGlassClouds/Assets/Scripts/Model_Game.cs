using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Game : MonoBehaviour {

    [Header("Shield")]
    public bool shieldOn;
    public Color insideColor_Base;
    public Color outsideColor_Base;
    public float hitHDRIntensity_Base;
    public float hitHDRIntensity_Boost;
    public float cutoff_Base;
    public float cutoff_Base_Upgrade;
    public float cutoff_Boost;
    public float cuttof_Boost_Upgrade;
    public Vector3 shieldForwardDirection;
    public float contractAndDialationRate;
    public float contractAndDialationRate_Upgrade;
    public float colorCoolDownLerpFraction;
    public float e_BlockedShotEnergyHit_Base;
    public float e_BlockedShotEnergyHit_Boost;
    public float e_ShieldEnergy_Actual;
    public AudioClip blockedShot_Base;
    public AudioClip blockedShot_Boost;
    [Header("Gun")]
    public bool gunOn;
    public float spinUpTime;
    public float spinUpTime_Upgrade;
    public float t_TimeBetweenShots;
    public float t_TimeBetweenShots_Upgrade;
    public Color gunTurretReticleColor;
    public float gunFiringReticleHDRBoost;
    public Color bulletColor;
    public AudioClip gunShot;
    public float e_GunEnergyRate_Passive;
    public float e_GunEnergyRate_Firing;
    public float e_GunEnergy_Actual;
    public float d_GunDamage_Base;
    public float d_GunDamage_Upgrade;
    [Header("Rockets")]
    public bool rocketsOn;
    public float someCannonFloat;
    public float e_RocketEnergyRate_Passive;
    public float e_RocketEnergyRate_Fire;
    public float e_RocketEnergy_Actual;
    public float t_RocketCooldown;
    public float d_ExplosionBallDamage;
    public float d_ExplosionBallSize_Base;
    public float d_ExplosionBallSize_Upgrade;
    [Header("Pilot")]
    public bool pilotOn;
    public float somePilotFloat;
    public float e_Flying;
    public float e_Flying_UpgradeDiscount;
    public float e_Jump;
    public float e_Jump_UpgradeDiscount;
    public float e_PilotEnergy_Actual;
    public float t_JumpCooldown;
    [Header("Sword")]
    public bool swordOn;
    public float someRepairFloat;
    public float e_SwordEnergyRate_Base;
    public float e_SwordEnergyRate_Boost;
    public float e_SwordEnergyRate_Boost_UpgradeDiscount;
    public float e_SwordEnergy_Actual;
    public float d_SwordDamage;
    public float d_SwordRange_Base;
    public float d_SwordRange_Upgrade;
    [Header("Reactor")]
    public float e_ReactorEnergyRate;
    public float e_ReactorEnergyRate_Upgrade;
    public float e_ShutdownThreshold;
    public float e_ShutdownThreshold_Upgrade;
    public float e_ReactorEnergy_Actual;
    public float t_shutDownTime;
    [Header("JumpDrive")]
    public float e_JumpActivateThreshold;
    public float e_JumpEnergy_Actual;
    [Header("Stations")]
    public Stations leftStation;
    public Stations rightStation;
}

public enum Stations { Pilot, Guns, Rockets, Shield, Sword }

