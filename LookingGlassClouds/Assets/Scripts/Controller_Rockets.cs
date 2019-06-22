using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Rockets : MonoBehaviour {

    public Model_Game gameModel;
    public Model_Input inputModel;

    public Transform player;
    private Transform rocketAim;
    private Transform rocketPitch;

    private MeshRenderer pointerReticle;

    // Use this for initialization
    void Start () {
        rocketAim = player.GetChild(2);
        rocketPitch = player.GetChild(2).GetChild(0);
        pointerReticle = rocketPitch.GetChild(0).GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (gameModel.leftStation == Stations.Rockets)
        {
            pointerReticle.enabled = true;
            _RocketStationAim(inputModel.L_Brg, inputModel.L_Mag);
            _Rockets(inputModel.L_Action_OnDown);
        }
        else if(gameModel.rightStation == Stations.Rockets)
        {
            pointerReticle.enabled = true;
            _RocketStationAim(inputModel.R_Brg, inputModel.R_Mag);
            _Rockets(inputModel.R_Action_OnDown);
        }
        else
        {
            pointerReticle.enabled = false;
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
        gameModel.rocketsOn = shoot;

        if (shoot)
            rocketIncrementor = 1;

        if (rocketIncrementor > 0)
        {
            GameObject g = Instantiate(rocket, transform.position, Quaternion.identity);
            g.GetComponent<Debug_RocketFlight>().ultimatePath = rocketPitch.up;
            rocketIncrementor++;
            if (rocketIncrementor >= gameModel.rocketCountMax)
                rocketIncrementor = 0;
        }
    }
    #endregion
}
