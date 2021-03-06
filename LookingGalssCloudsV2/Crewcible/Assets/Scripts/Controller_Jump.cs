﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Jump : SCG_Controller
{
    Model_Heat heatModel;
    Model_Play playModel;
    Model_Game gameModel;

    void Start()
    {
        heatModel = ServiceLocator.instance.Model.GetComponent<Model_Heat>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();

        playModel.jumpTotal = 15;


        priority = 8;
        Schedule(this);
    }

    bool penalty;
    bool lastPenalty;
    float prev;

    public override void ScheduledUpdate()
    {
        if (Mathf.Abs(heatModel.heat_Total - heatModel.max_HeatTotal) < 1)
        {
            penalty = true;
            playModel.jumpTotal -= playModel.jumpOverheatPenaltyRate * Time.deltaTime;
        }
        else
        {
            penalty = false;
            playModel.jumpTotal += playModel.jumpRateMax * heatModel.heatToChargeConversionFactor.Evaluate(heatModel.heat_Total / heatModel.max_HeatTotal) * Time.deltaTime;
        }

        if (penalty && !lastPenalty)
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.Danger));
        lastPenalty = penalty;

        playModel.jumpTotal = Mathf.Clamp(playModel.jumpTotal, 0, 100);

        if (playModel.jumpTotal <= 15 && prev > 15)
            SCG_EventManager.instance.Fire(new Event_Audio(AudioEvent.W_System));

        prev = playModel.jumpTotal;
    }
}
