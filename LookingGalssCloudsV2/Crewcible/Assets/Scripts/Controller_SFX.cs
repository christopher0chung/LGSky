using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_SFX : MonoBehaviour
{
    AudioSource gunSource;
    AudioSource shieldSource;
    public AudioSource shieldHumSource;
    public AudioSource lanceSource;
    AudioSource thrusterSource;
    AudioSource rocketSource;
    AudioSource oneShotSource;

    Manager_GameAssets assets;

    Model_Play playModel;
    Model_Game gameModel;
    Model_Heat heatModel;

    void Start()
    {
        assets = ServiceLocator.instance.Controller.GetComponent<Manager_GameAssets>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        heatModel = ServiceLocator.instance.Model.GetComponent<Model_Heat>();

        _MakeAndRefAudioSources();
        _SetClipsToAudioSources();

        SCG_EventManager.instance.Register<Event_PlayerShieldBlock>(EventHandler);
        SCG_EventManager.instance.Register<Event_EnemyBulletHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_PlayerExplode>(EventHandler);
    }

    void Update()
    {
        GunSounds();
        ThrusterSounds();
        LanceSounds();
        ShieldSounds();
    }

    public void EventHandler(SCG_Event e)
    {
        Event_EnemyBulletHit bH = e as Event_EnemyBulletHit;
        if (bH != null)
            oneShotSource.PlayOneShot(gameModel.sfx_EnemyBulletHit);

        Event_PlayerShieldBlock sB = e as Event_PlayerShieldBlock;
        if (sB != null)
            oneShotSource.PlayOneShot(gameModel.sfx_Shield_Block);

        Event_PlayerExplode pE = e as Event_PlayerExplode;
        if (pE != null)
            oneShotSource.PlayOneShot(gameModel.sfx_ShipExplode);
    }

    #region Guns
    bool lastGunsActive;
    float pitchLow = 1f;
    float pitchHigh = 1.2f;
    void GunSounds()
    {
        if (heatModel.active_Guns && !lastGunsActive && !heatModel.overheated_Guns)
        {
            gunSource.Play();
            Debug.Log("StartGunSounds");
        }
        else if (heatModel.active_Guns && !heatModel.overheated_Guns)
            gunSource.pitch = Mathf.Lerp(pitchLow, pitchHigh, heatModel.heat_Guns / 100);
        else
            gunSource.Stop();

        lastGunsActive = heatModel.active_Guns;

        //Debug.Log(heatModel.active_Guns + " " + lastGunsActive);
    }
    #endregion
    #region Thrusters
    float thrusters_MaxVolume = .5f;
    float thrusters_WorkingVolume;
    void ThrusterSounds()
    {
        if (playModel.leftStation == Stations.Thrusters || playModel.rightStation == Stations.Thrusters)
            thrusters_WorkingVolume = .1f + playModel.thrusterVelPerc * thrusters_MaxVolume;
        else
            thrusters_WorkingVolume = 0;

        thrusterSource.volume = Mathf.Lerp(thrusterSource.volume, thrusters_WorkingVolume, 6 * Time.deltaTime);

        if (heatModel.active_Dash)
            oneShotSource.PlayOneShot(gameModel.sfx_Thrusters_Dash);
    }
    #endregion
    #region Lance
    float lance_MaxVolume = .3f;
    float lance_WorkingVolume = 0;
    float lance_WorkingPitch = 1;
    bool lance_LastActive;

    bool lance_CurrentSelected;
    bool lance_LastSelected;
    void LanceSounds()
    {
        if (playModel.leftStation == Stations.Lance || playModel.rightStation == Stations.Lance)
        {
            if (heatModel.active_Lance)
            {
                lance_WorkingVolume = Mathf.Lerp(lance_WorkingVolume, lance_MaxVolume, Time.deltaTime * 5);
                lance_WorkingPitch = Mathf.Lerp(lance_WorkingPitch, 1.1f, Time.deltaTime * 5);
            }
            else
            {
                lance_WorkingVolume = Mathf.Lerp(lance_WorkingVolume, lance_MaxVolume / 2, Time.deltaTime * 5);
                lance_WorkingPitch = Mathf.Lerp(lance_WorkingPitch, 1f, Time.deltaTime * 5);
            }              
        }
        else
        {
            lance_WorkingVolume = Mathf.Lerp(lance_WorkingVolume, 0, Time.deltaTime * 7);
            lance_WorkingPitch = Mathf.Lerp(lance_WorkingPitch, 1f, Time.deltaTime * 7);
        }

        lanceSource.volume = lance_WorkingVolume;
        lanceSource.pitch = lance_WorkingPitch;

        if (playModel.leftStation == Stations.Lance || playModel.rightStation == Stations.Lance)
            lance_CurrentSelected = true;
        else
            lance_CurrentSelected = false;

        if (lance_CurrentSelected && !lance_LastSelected)
            oneShotSource.PlayOneShot(gameModel.sfx_LanceLit, .15f);

        lance_LastSelected = lance_CurrentSelected;

        if (heatModel.active_Lance && !lance_LastActive)
            oneShotSource.PlayOneShot(gameModel.sfx_LanceLit);

        lance_LastActive = heatModel.active_Lance;
    }
    #endregion
    #region Shield

    float shield_MaxVolume = .18f;
    float shield_WorkingVolume = 0;
    float shield_WorkingPitch = 1;
    void ShieldSounds()
    {
        if (playModel.leftStation == Stations.Shield || playModel.rightStation == Stations.Shield)
        {
            if (heatModel.active_Shield)
                shield_WorkingPitch = Mathf.Lerp(shield_WorkingPitch, 2, 7 * Time.deltaTime);
            else
                shield_WorkingPitch = Mathf.Lerp(shield_WorkingPitch, 1, 7 * Time.deltaTime);

            shield_WorkingVolume = Mathf.Lerp(shield_WorkingVolume, shield_MaxVolume, 7 * Time.deltaTime);
        }
        else
        {
            shield_WorkingPitch = Mathf.Lerp(shield_WorkingPitch, 1, 7 * Time.deltaTime);
            shield_WorkingVolume = Mathf.Lerp(shield_WorkingVolume, shield_MaxVolume, 7 * Time.deltaTime);
        }
        shieldHumSource.volume = shield_WorkingVolume;
        shieldHumSource.pitch = shield_WorkingPitch;
        shieldHumSource.transform.position = ServiceLocator.instance.Player.position + (Vector3)playModel.shieldDirection;
    }
    #endregion

    #region Setup
    void _MakeAndRefAudioSources()
    {
        Transform t = ServiceLocator.instance.Player;

        gunSource = t.gameObject.AddComponent<AudioSource>();
        shieldSource = t.gameObject.AddComponent<AudioSource>();
        thrusterSource = t.gameObject.AddComponent<AudioSource>();
        rocketSource = t.gameObject.AddComponent<AudioSource>();

        oneShotSource = ServiceLocator.instance.SFX;

        gunSource.playOnAwake = shieldSource.playOnAwake = lanceSource.playOnAwake = thrusterSource.playOnAwake = rocketSource.playOnAwake = oneShotSource.playOnAwake = false;
    }

    void _SetClipsToAudioSources()
    {
        gunSource.clip = gameModel.sfx_Gun_Shot;
        gunSource.loop = true;
        gunSource.volume = .25f;

        shieldSource.clip = gameModel.sfx_Shield_Block;
        shieldSource.loop = false;

        lanceSource.Play();

        thrusterSource.clip = gameModel.sfx_Thrusters_Jets;
        thrusterSource.loop = true;
        thrusterSource.volume = 0;
        thrusterSource.Play();

    }
    #endregion
}
