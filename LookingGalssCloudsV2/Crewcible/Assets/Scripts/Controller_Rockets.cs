using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Rockets : SCG_Controller {

    // Rockets also sets energy

    private Model_Game gameModel;
    private Model_Heat heatModel;
    private Model_Input inputModel;
    private Model_Play playModel;
    private Manager_GameAssets assetManager;
    private Transform player;

    private Transform rocketAim;
    private Transform rocketPitch;

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
    void Start () {
        rocketAim = player.Find("Reticles").Find("RocketReticle_Swivel");
        rocketPitch = rocketAim.Find("RocketReticle_Pitcher");

        priority = 3;
        Schedule(this);
    }
	
	// Update is called once per frame
	public override void ScheduledUpdate ()
    {
        playModel.rocketReloadProgress -= Time.deltaTime / gameModel.t_Rockets_Reload;
        playModel.rocketReloadProgress = Mathf.Clamp01(playModel.rocketReloadProgress);

        if (playModel.currentPlayerState == PlayerState.Alive ||
            playModel.currentPlayerState == PlayerState.Respawning)
        {
	        if (playModel.leftStation == Stations.Rockets)
            {
                _RocketStationAim(inputModel.L_Brg, inputModel.L_Mag);
                _Rockets(inputModel.L_Action_OnDown);
            }
            else if(playModel.rightStation == Stations.Rockets)
            {
                _RocketStationAim(inputModel.R_Brg, inputModel.R_Mag);
                _Rockets(inputModel.R_Action_OnDown);
            }
        }
    }

    #region Rockets

    //public GameObject rocket;

    private int rocketIncrementor;

    private void _RocketStationAim(float brg, float dec)
    {
        if (dec != 0)
            rocketAim.eulerAngles = new Vector3(0, -brg, 0);
        else
            rocketAim.eulerAngles = Vector3.zero;
        rocketPitch.localEulerAngles = new Vector3(dec, 0, 0);
    }

    private void _Rockets(bool shoot)
    {
        // From point of fire until reload complete, rockets will be charged the continuous heat penalty
        if (playModel.rocketReloadProgress >= 0.1f)
            heatModel.active_Rockets = true;
        else
            heatModel.active_Rockets = false;

        if (heatModel.active_Rockets)
            heatModel.heat_Rockets += heatModel.sustainedHeatRate_Rockets;

        // If player attepts to shoot when reload is complete...
        if (shoot && playModel.rocketReloadProgress == 0)
        {
            rocketIncrementor = 1;
            heatModel.heat_Rockets += heatModel.activationHeat_Rockets;
            playModel.rocketReloadProgress = 1;
        }

        // Once rockets start to fire...
        if (rocketIncrementor > 0)
        {
            GameObject g = assetManager.Make(MyGameAsset.Rocket, player.position);
            g.GetComponent<Behavior_Rockets>().Restart();
            //g.GetComponent<Behavior_Rocket>().ultimatePath = rocketPitch.up;
            rocketIncrementor++;
            if (rocketIncrementor >= gameModel.i_Rockets_RocketCountMax)
                rocketIncrementor = 0;
        }
    }
    #endregion
}
