using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Rockets : SCG_Controller {

    // Rockets also sets energy

    private Model_Game gameModel;
    private Model_Energy energyModel;
    private Model_Input inputModel;
    private Model_Play playModel;
    private Manager_GameAssets assetManager;
    private Transform player;

    private Transform rocketAim;
    private Transform rocketPitch;

    private List<MeshRenderer> reticleRenderers;
    //private MeshRenderer pointerReticle;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        energyModel = ServiceLocator.instance.Model.GetComponent<Model_Energy>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        player = ServiceLocator.instance.Player;

        assetManager = ServiceLocator.instance.Controller.GetComponent<Manager_GameAssets>();
    }

    // Use this for initialization
    void Start () {
        rocketAim = player.GetChild(2);
        rocketPitch = player.GetChild(2).GetChild(0);
        //pointerReticle = rocketPitch.GetChild(0).GetComponent<MeshRenderer>();
        reticleRenderers = new List<MeshRenderer>(rocketPitch.GetComponentsInChildren<MeshRenderer>());
    }
	
	// Update is called once per frame
	void Update ()
    {
        playModel.rocket_reloadProgress -= Time.deltaTime / gameModel.t_RocketCooldown;
        playModel.rocket_reloadProgress = Mathf.Clamp01(playModel.rocket_reloadProgress);

        if (playModel.currentPlayerState == PlayerState.Alive)
        {
	        if (gameModel.leftStation == Stations.Rockets)
            {
                //pointerReticle.enabled = true;
                foreach (MeshRenderer m in reticleRenderers)
                    m.enabled = true;
                _RocketStationAim(inputModel.L_Brg, inputModel.L_Mag);
                _Rockets(inputModel.L_Action_OnDown);
            }
            else if(gameModel.rightStation == Stations.Rockets)
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
        if (rocketIncrementor >= 1)
            energyModel.rocketsOn = true;
        else
            energyModel.rocketsOn = false;

        if (shoot && playModel.rocket_reloadProgress == 0)
        {
            rocketIncrementor = 1;
            energyModel.rocket_OpCost += gameModel.e_Rockets_Active;
            playModel.rocket_reloadProgress = 1;
        }

        if (rocketIncrementor > 0)
        {
            GameObject g = assetManager.Make(MyGameAsset.Rocket, player.position);
            g.GetComponent<Debug_RocketFlight>().ultimatePath = rocketPitch.up;
            rocketIncrementor++;
            if (rocketIncrementor >= gameModel.rocketCountMax)
                rocketIncrementor = 0;
        }
    }
    #endregion
}
