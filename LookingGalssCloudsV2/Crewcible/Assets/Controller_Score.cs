﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Score : MonoBehaviour
{
    Model_ScoreAndDifficulty sadModel;

    void Start()
    {
        sadModel = ServiceLocator.instance.Model.GetComponent<Model_ScoreAndDifficulty>();

        SCG_EventManager.instance.Register<Event_EnemyDeath>(EventHandler);
        SCG_EventManager.instance.Register<Event_PlayerBulletHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_PlayerRocketHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_PlayerSwordHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_PlayerShieldBlock>(EventHandler);
    }

    public void EventHandler (SCG_Event e)
    {
        Event_PlayerBulletHit bh = e as Event_PlayerBulletHit;

        if (bh != null)
        {
            sadModel.score += 7;
            return;
        }

        Event_PlayerShieldBlock sb = e as Event_PlayerShieldBlock;

        if (sb != null)
        {
            sadModel.score += 12;
            return;
        }

        Event_EnemyDeath ed = e as Event_EnemyDeath;

        if (ed != null)
        {
            sadModel.score += 100;
            return;
        }

        Event_PlayerRocketHit rh = e as Event_PlayerRocketHit;

        if (rh != null)
        {
            sadModel.score += 21;
            return;
        }

        Event_PlayerSwordHit sh = e as Event_PlayerSwordHit;

        if (sh != null)
        {
            sadModel.score += 3;
        }
    }
}
