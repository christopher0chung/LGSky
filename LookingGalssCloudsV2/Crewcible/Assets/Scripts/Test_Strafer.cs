using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Strafer : Behavior_BaddyBase
{
    public Transform turret;

    private Transform target;

    private Vector3 projection;

    private float shootTimer;
    public GameObject bullet;
    public GameObject flameOut;

    Enemy_Base myE;

    ParticleSystem damageInd;

    void Start()
    {
        GrabStdRefs();
        target = ServiceLocator.instance.Player;
        SCG_EventManager.instance.Register<Event_EnemyDeath>(EnemyDeathEventHandler);
        SCG_EventManager.instance.Register<Event_DumpReg>(EnemyDeathEventHandler);
        myE = GetComponent<Enemy_Base>();
        damageInd = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        projection = target.position;
        projection.y = turret.position.y;
        turret.rotation = Quaternion.LookRotation(projection - turret.position, transform.up);
        transform.position += (transform.forward * 37 + Vector3.forward * -30) * Time.deltaTime + WorldEffectOffset();

        shootTimer += Time.deltaTime;
        if (shootTimer >= .75f)
        {
            shootTimer = 0;
            if (turret.position.z > target.position.z + 12 && turret.position.z < 100)
                Instantiate(bullet, turret.position + turret.forward * 1.1f, Quaternion.LookRotation(target.position - turret.position, transform.up));
        }

        if (transform.position.z >= 270 || transform.position.z <= -150)
        {
            SCG_EventManager.instance.Unregister<Event_EnemyDeath>(EnemyDeathEventHandler);
            Destroy(this.gameObject);
        }

        var rate = damageInd.emission;
        rate.rateOverTime = 50 * (myE.hitpoints_Max - myE.hitpoints_Current) / myE.hitpoints_Max;

        if (transform.position.z <= -200)
        {
            SCG_EventManager.instance.Unregister<Event_EnemyDeath>(EnemyDeathEventHandler);
            SCG_EventManager.instance.Unregister<Event_DumpReg>(EnemyDeathEventHandler);
            Destroy(this.gameObject);
        }
    }

    public void EnemyDeathEventHandler(SCG_Event e)
    {
        Event_EnemyDeath eD = e as Event_EnemyDeath;

        if (eD != null)
        {
            if (eD.enemyToBeDestroyed != myE)
                return;

            SCG_EventManager.instance.Unregister<Event_EnemyDeath>(EnemyDeathEventHandler);
            SCG_EventManager.instance.Unregister<Event_DumpReg>(EnemyDeathEventHandler);
            SCG_EventManager.instance.Fire(new Event_BonusPoints(1203));
            Instantiate(flameOut, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        Event_DumpReg d = e as Event_DumpReg;
        if (d != null)
        {
            SCG_EventManager.instance.Unregister<Event_EnemyDeath>(EnemyDeathEventHandler);
            SCG_EventManager.instance.Unregister<Event_DumpReg>(EnemyDeathEventHandler);
        }
    }
}
