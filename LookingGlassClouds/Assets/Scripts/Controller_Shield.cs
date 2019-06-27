using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Shield : SCG_Controller {

    private Model_Game gameModel;
    private Model_Energy energyModel;
    private Model_Input inputModel;
    private Model_Play playModel;

    public Material shieldMat;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        energyModel = ServiceLocator.instance.Model.GetComponent<Model_Energy>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
    }

	void Update () {
        if (playModel.currentPlayerState == PlayerState.Alive)
        {
            if (gameModel.leftStation == Stations.Shield)
            {
                _ShieldCalculateAndMove(inputModel.L_X, inputModel.L_Y);
                _ShieldSize(inputModel.L_Action_Down);
            }
            else if (gameModel.rightStation == Stations.Shield)
            {
                _ShieldCalculateAndMove(inputModel.R_X, inputModel.R_Y);
                _ShieldSize(inputModel.R_Action_Down);
            }
            else
            {
                energyModel.shieldSize_Cutoff = 1.1f;
                shieldMat.SetFloat("_Cutoff", 1.1f);
            }
        }
        else
        {
            energyModel.shieldSize_Cutoff = 1.1f;
            shieldMat.SetFloat("_Cutoff", 1.1f);
        }
    }

    #region Shield

    private void _ShieldCalculateAndMove(float inputX, float inputY)
    {
        gameModel.shieldForwardDirection.x = inputX;
        gameModel.shieldForwardDirection.z = inputY;
        gameModel.shieldForwardDirection.y = 1 - (Mathf.Sqrt(inputX * inputX + inputY * inputY));
        shieldMat.SetVector("_Forward", gameModel.shieldForwardDirection);
    }

    private void _ShieldSize(bool on)
    {
        energyModel.shieldOn = on;

        if (on)
            energyModel.shieldSize_Cutoff = gameModel.cutoff_Boost;
        else
            energyModel.shieldSize_Cutoff = gameModel.cutoff_Base;

        shieldMat.SetFloat("_Cutoff", energyModel.shieldSize_Cutoff);
    }

    #endregion
}
