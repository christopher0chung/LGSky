using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour {

    Model_Game gameModel;

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
                        if (myDeathExplosion != null)
                        {
                            GameObject e = Instantiate(myDeathExplosion, transform.position, Quaternion.identity);
                            e.GetComponent<Behavior_EffectWorldScroll>().Fire(transform.forward);
                        }

                        SCG_EventManager.instance.Fire(new Event_EnemyDeath(this, transform.position));
                        //Debug.Log("I should be dead");
                    }
                }
            }
        }
    }

    public float hitpoints_Max;

    public GameObject myDeathExplosion;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();

        SCG_EventManager.instance.Register<Event_PlayerBulletHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_LanceHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_ExplosionBallHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_PlayerRocketHit>(EventHandler);
        this.tag = "Enemy";
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody r = gameObject.AddComponent<Rigidbody>();
            r.isKinematic = true;
            r.useGravity = false;
        }
    }

    public void SetHitPoint(float max)
    {
        hitpoints_Max = max;
        hitpoints_Current = max;
    }

    private void EventHandler(SCG_Event e)
    {
        Event_PlayerBulletHit p = e as Event_PlayerBulletHit;

        if (p != null)
        {
            if (p.enemyHit == this)
            {
                hitpoints_Current -= p.enemyDamageTaken;
            }
        }

        Event_LanceHit s = e as Event_LanceHit;

        if (s != null)
        {
            if (s.enemyHit == this)
            {
                if (s.trueEnterFalseExit)
                    hitpoints_Current -= gameModel.d_Lance_Damage;
                isInLance = s.trueEnterFalseExit;
            }
        }

        Event_ExplosionBallHit x = e as Event_ExplosionBallHit;

        if (x != null)
        {
            if (x.enemyHit == this)
            {
                isInExplosionBall = x.trueEnterFalseExit;
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

    bool isInLance;
    bool isInExplosionBall;

    public void Update()
    {
        if (isInLance)
            hitpoints_Current -= gameModel.d_Lance_Damage_Sustained * Time.deltaTime;
        if (isInExplosionBall)
            hitpoints_Current -= gameModel.d_Rockets_ExplosionBallDamage * Time.deltaTime;
    }

    //public void FireDestructionEvent()
    //{
    //    hitpoints_Current = 0;
    //}
}
