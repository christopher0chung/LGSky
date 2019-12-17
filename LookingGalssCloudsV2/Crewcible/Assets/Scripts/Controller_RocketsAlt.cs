using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_RocketsAlt : SCG_Controller
{
    private Model_Game gameModel;
    private Model_Heat heatModel;
    private Model_Input inputModel;
    private Model_Play playModel;
    private Manager_GameAssets assetManager;
    private Transform player;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        heatModel = ServiceLocator.instance.Model.GetComponent<Model_Heat>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        player = ServiceLocator.instance.Player;

        assetManager = ServiceLocator.instance.Controller.GetComponent<Manager_GameAssets>();
    }

    // Use this for initialization
    void Start()
    {
        priority = 3;
        Schedule(this);
    }

    // Update is called once per frame
    public override void ScheduledUpdate()
    {
        playModel.rocketReloadProgress -= Time.deltaTime / gameModel.t_Rockets_Reload;
        playModel.rocketReloadProgress = Mathf.Clamp01(playModel.rocketReloadProgress);

        if (playModel.currentPlayerState == PlayerState.Alive ||
            playModel.currentPlayerState == PlayerState.Respawning)
        {
            if (playModel.leftStation == Stations.Rockets)
            {
                _Rockets(inputModel.L_Action_OnDown);
            }
            else if (playModel.rightStation == Stations.Rockets)
            {
                _Rockets(inputModel.R_Action_OnDown);
            }
            _RocketFiring();
        }
        else
        {
            heatModel.active_Rockets = false;
        }
    }

    #region Rockets

    //public GameObject rocket;

    private int rocketIncrementor;

    private float rocketTimer;

    private void _Rockets(bool shoot)
    {
        // From point of fire until reload complete, rockets will be charged the continuous heat penalty
        if (playModel.rocketReloadProgress >= 0.1f)
            heatModel.active_Rockets = true;
        else
            heatModel.active_Rockets = false;

        // If player attepts to shoot when reload is complete...
        if (shoot && playModel.rocketReloadProgress == 0 && !heatModel.overheated_Rockets)
        {
            rocketIncrementor = 1;
            //heatModel.heat_Rockets += heatModel.activationHeat_Rockets;
            playModel.rocketReloadProgress = 1;
            rocketTimer = 0;
        }
    }

    private void _RocketFiring()
    {
        // Once rockets start to fire...
        // Will only allow to fire rockets if not overheated
        // If overheated, the rocket incrementor will be reset
        // Once not overheated, the incrementor's control is released back to _Rockets()
        if (rocketIncrementor > 0 && !heatModel.overheated_Rockets)
        {
            rocketTimer += Time.deltaTime;
            if (rocketTimer >= gameModel.t_Rockets_FireRate)
            {
                rocketTimer -= gameModel.t_Rockets_FireRate;

                Vector3 newRocketDir = Vector3.Normalize(Random.insideUnitSphere);
                Vector3 newRocketPos = player.position + newRocketDir;

                GameObject g = assetManager.Make(MyGameAsset.SFX, newRocketPos);
                g.GetComponent<AudioSource>().PlayOneShot(gameModel.sfx_RocketLaunch);

                g = assetManager.Make(MyGameAsset.Rocket, newRocketPos);
                g.GetComponent<Behavior_RocketsAlt>().Restart(newRocketPos, newRocketDir);
                //g.GetComponent<Behavior_Rocket>().ultimatePath = rocketPitch.up;
                rocketIncrementor++;
            }

            if (rocketIncrementor >= gameModel.i_Rockets_RocketCountMax || (playModel.leftStation != Stations.Rockets && playModel.rightStation != Stations.Rockets))
                rocketIncrementor = 0;
        }
        else if (heatModel.overheated_Rockets || playModel.currentPlayerState != PlayerState.Alive)
            rocketIncrementor = 0;
    }
    #endregion
}
