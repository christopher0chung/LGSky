﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour {

    public float hitpoints_Max;
    public float hitpoints_Current;

    void Awake()
    {
        SCG_EventManager.instance.Register<Event_PlayerBulletHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_PlayerSwordHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_ExplosionBallHit>(EventHandler);
    }

    public void SetHitPoint(float max)
    {
        hitpoints_Max = max;
        hitpoints_Current = hitpoints_Max;
    }

    private void EventHandler(SCG_Event e)
    {
        Event_PlayerBulletHit p = e as Event_PlayerBulletHit;

        if (p != null)
        {
            if (p.enemyHit == this)
            {
                //Debug.Log("Starting with " + hitpoints_Current);
                hitpoints_Current -= p.enemyDamageTaken;
                //Debug.Log("Ending up with " + hitpoints_Current);
                if (hitpoints_Current <= 0)
                    SCG_EventManager.instance.Fire(new Event_EnemyDeath(this, transform.position));
            }
        }

        Event_PlayerSwordHit s = e as Event_PlayerSwordHit;

        if (s != null)
        {
            if (s.enemyHit == this)
            {
                hitpoints_Current -= s.enemyDamageTaken;
                if (hitpoints_Current <= 0)
                    SCG_EventManager.instance.Fire(new Event_EnemyDeath(this, transform.position));
            }
        }

        Event_ExplosionBallHit x = e as Event_ExplosionBallHit;

        if (x != null)
        {
            if (x != null)
            {
                if (x.enemyHit == this)
                {
                    hitpoints_Current -= x.enemyDamageTaken;
                    if (hitpoints_Current <= 0)
                        SCG_EventManager.instance.Fire(new Event_EnemyDeath(this, transform.position));
                }
            }
        }
    }
}