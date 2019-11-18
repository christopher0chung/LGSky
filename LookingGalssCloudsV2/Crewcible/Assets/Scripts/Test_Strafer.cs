﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Strafer : MonoBehaviour
{
    public Transform turret;

    private Transform target;

    private Vector3 projection;

    private float shootTimer;
    public GameObject bullet;

    Enemy_Base myE;

    ParticleSystem damageInd;

    void Start()
    {
        target = ServiceLocator.instance.Player;
        SCG_EventManager.instance.Register<Event_EnemyDeath>(EnemyDeathEventHandler);
        myE = GetComponent<Enemy_Base>();
        damageInd = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        projection = target.position;
        projection.y = turret.position.y;
        turret.rotation = Quaternion.LookRotation(projection - turret.position, transform.up);
        transform.position += (transform.forward * 37 + Vector3.forward * -30) * Time.deltaTime;

        shootTimer += Time.deltaTime;
        if (shootTimer >= .75f)
        {
            shootTimer = 0;
            if (turret.position.z > target.position.z + 12 && turret.position.z < 100)
                Instantiate(bullet, turret.position + turret.forward * 1.1f, Quaternion.LookRotation(target.position - turret.position, transform.up));
        }

        if (transform.position.z >= 270)
            Destroy(this.gameObject);

        var rate = damageInd.emission;
        rate.rateOverTime = 50 * (myE.hitpoints_Max - myE.hitpoints_Current) / myE.hitpoints_Max;
    }

    public void EnemyDeathEventHandler(SCG_Event e)
    {
        Event_EnemyDeath eD = e as Event_EnemyDeath;

        if (eD != null)
        {
            if (eD.enemyToBeDestroyed != myE)
                return;

            SCG_EventManager.instance.Fire(new Event_BonusPoints(1203));
            Destroy(this.gameObject);
        }
    }
}
