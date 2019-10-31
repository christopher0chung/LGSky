using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Lance : SCG_Controller
{
    private Model_Heat heatModel;
    private Model_Play playModel;
    private Model_Input inputModel;
    private Model_Game gameModel;

    private Transform lanceSwivel;
    private Transform lancePitcher;
    private Transform lance;
    private ParticleSystem lanceBloom;
    private Collider lanceCollider;

    private Vector3 currentSize;
    private Vector3 targetSize;
    private void Awake()
    {
        heatModel = ServiceLocator.instance.Model.GetComponent<Model_Heat>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
    }

    void Start()
    {
        lanceSwivel = ServiceLocator.instance.Player.Find("ShipParent").Find("ThermalLance_Swivel");
        lancePitcher = lanceSwivel.GetChild(0);
        lance = lancePitcher.GetChild(0);
        lanceBloom = lance.GetChild(2).GetComponent<ParticleSystem>();
        lanceCollider = lance.GetComponent<Collider>();

        priority = 2;
        Schedule(this);
    }

    public override void ScheduledUpdate()
    {
        if (playModel.currentPlayerState == PlayerState.Alive ||
            playModel.currentPlayerState == PlayerState.Respawning)
        {
            if (playModel.leftStation == Stations.Lance)
                _Lance(inputModel.L_Brg, inputModel.L_Mag, inputModel.L_Action_Down);
            else if (playModel.rightStation == Stations.Lance)
                _Lance(inputModel.R_Brg, inputModel.R_Mag, inputModel.R_Action_Down);
            else
            {
                lance.localScale = Vector3.zero;
                lanceCollider.enabled = false;
                if (lanceBloom.isPlaying)
                    lanceBloom.Stop();
            }
        }
        else
        {
            lance.localScale = Vector3.zero;
            lanceCollider.enabled = false;
            if (lanceBloom.isPlaying)
                lanceBloom.Stop();
        }
    }

    #region Lance

    private void _Lance(float brg, float dec, bool extend)
    {
        heatModel.active_Lance = extend;

        lanceSwivel.eulerAngles = new Vector3(0, -brg, 0);
        lancePitcher.localEulerAngles = new Vector3(dec, 0, 0);

        if (extend && !heatModel.overheated_Lance)
        {
            targetSize.x = targetSize.z = gameModel.f_Lance_MinRange;
            targetSize.y = gameModel.f_Lance_MaxRange;
        }
        else
        {
            targetSize = Vector3.one * gameModel.f_Lance_MinRange;
        }

        currentSize = Vector3.Lerp(currentSize, targetSize, gameModel.s_Lance_ExtendSpeed * Time.deltaTime);
        lance.localScale = currentSize;

        lanceCollider.enabled = true;
        if (!lanceBloom.isPlaying)
            lanceBloom.Play();
    }
    #endregion
}
