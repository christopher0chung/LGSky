using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_SFX : MonoBehaviour
{
    AudioSource gunSource;
    AudioSource shieldSource;
    AudioSource lanceSource;
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
    }

    void Update()
    {
        GunSounds();
        ThrusterSounds();
    }

    public void EventHandler(SCG_Event e)
    {
        Event_EnemyBulletHit bH = e as Event_EnemyBulletHit;
        if (bH != null)
            oneShotSource.PlayOneShot(gameModel.sfx_EnemyBulletHit);

        Event_PlayerShieldBlock sB = e as Event_PlayerShieldBlock;
        if (sB != null)
            oneShotSource.PlayOneShot(gameModel.sfx_Shield_Block);
    }

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

        Debug.Log(heatModel.active_Guns + " " + lastGunsActive);
    }

    float maxVolume = .5f;
    float workingVolume;
    void ThrusterSounds()
    {
        if (playModel.leftStation == Stations.Thrusters || playModel.rightStation == Stations.Thrusters)
            workingVolume = .1f + playModel.thrusterVelPerc * maxVolume;
        else
            workingVolume = 0;

        thrusterSource.volume = Mathf.Lerp(thrusterSource.volume, workingVolume, 6 * Time.deltaTime);

        if (heatModel.active_Dash)
            oneShotSource.PlayOneShot(gameModel.sfx_Thrusters_Dash);
    }

    void _MakeAndRefAudioSources()
    {
        Transform t = ServiceLocator.instance.Player;

        gunSource = t.gameObject.AddComponent<AudioSource>();
        shieldSource = t.gameObject.AddComponent<AudioSource>();
        lanceSource = t.gameObject.AddComponent<AudioSource>();
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

        thrusterSource.clip = gameModel.sfx_Thrusters_Jets;
        thrusterSource.loop = true;
        thrusterSource.volume = 0;
        thrusterSource.Play();

    }
}
