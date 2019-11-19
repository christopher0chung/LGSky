using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Jump : SCG_Controller
{
    Model_Heat heatModel;
    Model_Play playModel;
    Model_Game gameModel;

    public AnimationCurve heatToChargeConversionFactor;

    private void Awake()
    {
       SCG_EventManager.instance.Register<Event_EnemyBulletHit>(DamageEventHandler);
    }
    void Start()
    {
        heatModel = ServiceLocator.instance.Model.GetComponent<Model_Heat>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();

        playModel.jumpTotal = 15;


        priority = 8;
        Schedule(this);
    }

    public override void ScheduledUpdate()
    {
        if (Mathf.Abs(heatModel.heat_Total - heatModel.max_HeatTotal) < 1)
            playModel.jumpTotal -= playModel.jumpOverheatPenaltyRate * Time.deltaTime;
        else
            playModel.jumpTotal += playModel.jumpRateMax * heatToChargeConversionFactor.Evaluate(heatModel.heat_Total / heatModel.max_HeatTotal) * Time.deltaTime;

        playModel.jumpTotal = Mathf.Clamp(playModel.jumpTotal, 0, 100);
    }

    public void DamageEventHandler(SCG_Event e)
    {
        Event_EnemyBulletHit bH = e as Event_EnemyBulletHit;
        if (bH != null)
            playModel.jumpTotal -= gameModel.m_EnemyBulletDamage;

        playModel.jumpTotal = Mathf.Clamp(playModel.jumpTotal, 0, 100);
    }
}
