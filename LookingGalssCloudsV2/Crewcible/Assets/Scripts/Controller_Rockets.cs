using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Rockets : SCG_Controller {

    // Rockets also sets energy

    private Model_Game gameModel;
    private Model_Heat heatModel;
    private Model_Input inputModel;
    private Model_Play playModel;
    //private Manager_GameAssets assetManager;
    private Transform player;

    private Transform rocketAim;
    private Transform rocketPitch;

    public List<MeshRenderer> reticleRenderers;
    //private MeshRenderer pointerReticle;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        heatModel = ServiceLocator.instance.Model.GetComponent<Model_Heat>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        player = ServiceLocator.instance.Player;

        //assetManager = ServiceLocator.instance.Controller.GetComponent<Manager_GameAssets>();
    }

    // Use this for initialization
    void Start () {
        rocketAim = player.Find("Reticles").Find("RocketReticle_Swivel");
        rocketPitch = rocketAim.Find("RocketReticle_Pitcher");
        //pointerReticle = rocketPitch.GetChild(0).GetComponent<MeshRenderer>();
        reticleRenderers = new List<MeshRenderer>(rocketPitch.GetComponentsInChildren<MeshRenderer>());
    }
	
	// Update is called once per frame
	void Update ()
    {
        playModel.rocketReloadProgress -= Time.deltaTime / gameModel.t_Rockets_Reload;
        playModel.rocketReloadProgress = Mathf.Clamp01(playModel.rocketReloadProgress);

        if (playModel.currentPlayerState == PlayerState.Alive ||
            playModel.currentPlayerState == PlayerState.Respawning)
        {
	        if (playModel.leftStation == Stations.Rockets)
            {
                //pointerReticle.enabled = true;
                foreach (MeshRenderer m in reticleRenderers)
                    m.enabled = true;
                _RocketStationAim(inputModel.L_Brg, inputModel.L_Mag);
                _Rockets(inputModel.L_Action_OnDown);
            }
            else if(playModel.rightStation == Stations.Rockets)
            {
                //pointerReticle.enabled = true;
                foreach (MeshRenderer m in reticleRenderers)
                    m.enabled = true;
                _RocketStationAim(inputModel.R_Brg, inputModel.R_Mag);
                _Rockets(inputModel.R_Action_OnDown);
            }
            else
            {
                //pointerReticle.enabled = false;
                foreach (MeshRenderer m in reticleRenderers)
                    m.enabled = false;
            }
        }
        else
        {
            //pointerReticle.enabled = false;
            foreach (MeshRenderer m in reticleRenderers)
                m.enabled = false;
        }
    }

    #region Rockets

    public GameObject rocket;

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
        if (playModel.rocketReloadProgress >= 0)
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
            //GameObject g = assetManager.Make(MyGameAsset.Rocket, player.position);
            //g.GetComponent<Debug_RocketFlight>().ultimatePath = rocketPitch.up;
            rocketIncrementor++;
            if (rocketIncrementor >= gameModel.i_Rockets_RocketCountMax)
                rocketIncrementor = 0;
        }
    }
    #endregion
}
