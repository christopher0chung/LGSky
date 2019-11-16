using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour {

    private float _h = 1;
    public float hitpoints_Current
    {
        get
        {
            return _h;
        }
        set
        {
            if (value != _h)
            {
                _h = value;
                if (_h <= 0)
                {
                    if (hitpoints_Current <= 0)
                    {
                        SCG_EventManager.instance.Fire(new Event_EnemyDeath(this, transform.position));
                        Debug.Log("I should be dead");
                    }
                }
            }
        }
    }
    public float hitpoints_Max;

    void Awake()
    {
        SCG_EventManager.instance.Register<Event_PlayerBulletHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_PlayerSwordHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_ExplosionBallHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_PlayerRocketHit>(EventHandler);
        this.tag = "Enemy";
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody r = gameObject.AddComponent<Rigidbody>();
            r.isKinematic = true;
        }
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
                hitpoints_Current -= p.enemyDamageTaken;
                //Debug.Log("I've been hit. My Life is now at " + hitpoints_Current);
            }
        }

        Event_PlayerSwordHit s = e as Event_PlayerSwordHit;

        if (s != null)
        {
            if (s.enemyHit == this)
            {
                hitpoints_Current -= s.enemyDamageTaken;
            }
        }

        Event_ExplosionBallHit x = e as Event_ExplosionBallHit;

        if (x != null)
        {
            if (x.enemyHit == this)
            {
                hitpoints_Current -= x.enemyDamageTaken;
            }
        }

        Event_PlayerRocketHit r = e as Event_PlayerRocketHit;

        if (r != null)
        {
            if (r.enemyHit == this)
            {
                hitpoints_Current -= r.damageTaken;
            }
        }
    }

    public void FireDestructionEvent()
    {
        hitpoints_Current = 0;
    }
}
