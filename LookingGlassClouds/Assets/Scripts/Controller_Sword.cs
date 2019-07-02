using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Sword : SCG_Controller {

    private Model_Game gameModel;
    private Model_Energy energyModel;
    private Model_Input inputModel;
    private Model_Play playModel;

    public Transform swordPivot;
    private Transform swordPivot2;
    public Transform swd;
    private Collider swdColl;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        energyModel = ServiceLocator.instance.Model.GetComponent<Model_Energy>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
    }

    void Start()
    {
        swordPivot2 = swordPivot.GetChild(0);
        swdColl = swd.GetComponent<Collider>();
    }

	void Update () {
        if (playModel.currentPlayerState == PlayerState.Alive ||
            playModel.currentPlayerState == PlayerState.Respawning)
        {
            if (gameModel.leftStation == Stations.Sword)
                _Sword(inputModel.L_Brg, inputModel.L_Mag, inputModel.L_Action_Down);
            else if (gameModel.rightStation == Stations.Sword)
                _Sword(inputModel.R_Brg, inputModel.R_Mag, inputModel.R_Action_Down);
            else
            {
                swd.localScale = Vector3.zero;
                swdColl.enabled = false;
            }
        }
        else
        {
            swd.localScale = Vector3.zero;
            swdColl.enabled = false;
        }
	}

    #region Sword

    private Vector3 bigScale = new Vector3(.2f, 15, .2f);
    private Vector3 bigPos = new Vector3(0, 21.5f, 0);

    private Vector3 smallScale = new Vector3(.2f, 5, .2f), currentScale = new Vector3(.2f, 5, .2f);
    private Vector3 smallPos = new Vector3(0, 12, 0), currentPos = new Vector3(0, 12, 0);

    private void _Sword(float brg, float dec, bool extend)
    {
        energyModel.swordOn = extend;

        swordPivot.eulerAngles = new Vector3(0, -brg, 0);
        swordPivot2.localEulerAngles = new Vector3(dec, 0, 0);

        if (extend)
        {
            currentScale = Vector3.Lerp(currentScale, bigScale, 0.8f);
            currentPos = Vector3.Lerp(currentPos, bigPos, 0.8f);
        }
        else
        {
            currentScale = Vector3.Lerp(currentScale, smallScale, 0.8f);
            currentPos = Vector3.Lerp(currentPos, smallPos, 0.8f);
        }

        swd.localScale = currentScale;
        swd.localPosition = currentPos;
        swdColl.enabled = true;
    }
    #endregion
}
