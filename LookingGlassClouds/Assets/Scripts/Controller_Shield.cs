using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Shield : MonoBehaviour {

    public Model_Game gameModel;
    public Model_Input inputModel;
    public Material shieldMat;

	void Update () {
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
            shieldMat.SetFloat("_Cutoff", 1);
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
        gameModel.shieldOn = on;

        if (on)
            shieldMat.SetFloat("_Cutoff", gameModel.cutoff_Boost);
        else 
            shieldMat.SetFloat("_Cutoff", gameModel.cutoff_Base);
    }

    #endregion
}
