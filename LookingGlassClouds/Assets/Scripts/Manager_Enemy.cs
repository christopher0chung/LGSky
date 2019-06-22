using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Enemy : MonoBehaviour {
    private Asset_BoidsManager bm;

    public Model_Game gameModel;

    public List<Enemy_Base> activeEnemies;
    public List<Enemy_Base> pendingDestroy;

    public void Awake()
    {
        activeEnemies = new List<Enemy_Base>();
        pendingDestroy = new List<Enemy_Base>();

        bm = GetComponent<Asset_BoidsManager>();

        SCG_EventManager.instance.Register<Event_EnemyDeath>(EnemyDeathHandler);
    }

    public void EnemyDeathHandler(SCG_Event e)
    {
        Event_EnemyDeath d = e as Event_EnemyDeath;

        if (d != null)
        {
            if (activeEnemies.Contains(d.enemyToBeDestroyed))
            {
                Debug.Log("Active Enemy Being Destroyed");
                activeEnemies.Remove(d.enemyToBeDestroyed);
                pendingDestroy.Add(d.enemyToBeDestroyed);
            }
        }
    }

    public void Spawn(EnemyType type, Vector3 position, int count)
    {
        if (type == EnemyType.SwarmBoys)
        {
            List<Asset_Boid> flockOfSwarmBoys = new List<Asset_Boid>();
            for (int i = 0; i < count; i++)
            {
                Debug.Assert(gameModel.swarmBoyPrefab != null, "Where is SwarmBoyPrefab?");
                GameObject g = Instantiate<GameObject>(gameModel.swarmBoyPrefab, position, Quaternion.identity, null);
                Debug.Assert(g != null, "Where's my G?!");
                Enemy_Base enemy = g.GetComponent<Enemy_Base>();
                enemy.SetHitPoint(100);
                activeEnemies.Add(enemy);
                flockOfSwarmBoys.Add(g.GetComponent<Asset_Boid>());
            }

            bm.RegisterFlock(flockOfSwarmBoys);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Spawn(EnemyType.SwarmBoys, Vector3.forward * 40, 20);
        }
    }

    public void FixedUpdate()
    {
        if (pendingDestroy.Count > 0)
        {
            for (int i = pendingDestroy.Count - 1; i >= 0; i--)
            {
                Enemy_Base e = pendingDestroy[i];
                pendingDestroy.Remove(e);
                Destroy(e.gameObject);
            }
        }
    }
}

public enum EnemyType { SwarmBoys }
