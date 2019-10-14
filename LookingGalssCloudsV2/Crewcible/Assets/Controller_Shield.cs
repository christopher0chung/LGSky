using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Shield : SCG_Controller {

    private Model_Game gameModel;
    private Model_Heat heatModel;
    private Model_Input inputModel;
    private Model_Play playModel;

    public Material shieldMat;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        heatModel = ServiceLocator.instance.Model.GetComponent<Model_Heat>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
    }

    private void Start()
    {
        priority = 2;
        Schedule(this);
    }

    public override void ScheduledUpdate () {
        if (playModel.currentPlayerState == PlayerState.Alive ||
            playModel.currentPlayerState == PlayerState.Respawning)
        {
            if (playModel.leftStation == Stations.Shield)
            {
                _ShieldCalculateAndMove(inputModel.L_X, inputModel.L_Y);
                _ShieldSize(inputModel.L_Action_Down);
            }
            else if (playModel.rightStation == Stations.Shield)
            {
                _ShieldCalculateAndMove(inputModel.R_X, inputModel.R_Y);
                _ShieldSize(inputModel.R_Action_Down);
            }
            else
            {
                playModel.shieldSize = 1.1f;
                shieldMat.SetFloat("_Cutoff", 1.1f);
            }
        }
        else
        {
            playModel.shieldSize = 1.1f;
            shieldMat.SetFloat("_Cutoff", 1.1f);
        }
    }

    #region Shield
    private void _ShieldCalculateAndMove(float inputX, float inputY)
    {
        playModel.shieldDirection.x = inputX;
        playModel.shieldDirection.z = inputY;
        playModel.shieldDirection.y = 1 - (Mathf.Sqrt(inputX * inputX + inputY * inputY));
        shieldMat.SetVector("_Forward", playModel.shieldDirection);
    }

    private void _ShieldSize(bool on)
    {
        heatModel.active_Shield = on;

        if (on)
            playModel.shieldSize = gameModel.f_Shield_Cutoff_Max;
        else
            playModel.shieldSize = gameModel.f_Shield_Cutoff_Min;

        shieldMat.SetFloat("_Cutoff", playModel.shieldSize);
    }
    #endregion
}
