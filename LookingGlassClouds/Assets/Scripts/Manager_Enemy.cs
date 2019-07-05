﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Enemy : MonoBehaviour {
    private Asset_BoidsManager bm;

    private Model_Game gameModel;

    public List<Enemy_Base> activeEnemies;
    public List<Enemy_Base> pendingDestroy;

    private int height = 3;

    public void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();

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
                GameObject g = Instantiate<GameObject>(gameModel.swarmBoyPrefab, position, Quaternion.identity, transform);
                Debug.Assert(g != null, "Where's my G?!");
                Enemy_Base enemy = g.GetComponent<Enemy_Base>();
                enemy.SetHitPoint(gameModel.hp_SwarmBoy);
                activeEnemies.Add(enemy);
                flockOfSwarmBoys.Add(g.GetComponent<Asset_Boid>());
            }

            bm.RegisterFlock(flockOfSwarmBoys);
        }
        else if (type == EnemyType.RingDudes)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject g = Instantiate<GameObject>(gameModel.ringDudePrefab, position + UnityEngine.Random.insideUnitSphere * 10, Quaternion.identity, transform);
                Enemy_Base enemy = g.GetComponent<Enemy_Base>();
                enemy.SetHitPoint(gameModel.hp_RingDude);
                activeEnemies.Add(enemy);
                Behavior_RingDudes bRD = g.GetComponent<Behavior_RingDudes>();
                bRD.Init(i, count, height);
            }
            height++;
            if (height > 6)
                height = 3;
        }
        else if (type == EnemyType.Mines)
        {
            GameObject g = Instantiate<GameObject>(gameModel.minePrefab, position, Quaternion.identity, transform);
            Enemy_Base enemy = g.GetComponent<Enemy_Base>();
            enemy.SetHitPoint(gameModel.hp_Mine);
            activeEnemies.Add(enemy);
        }
        else if (type == EnemyType.Missiles)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject g = Instantiate<GameObject>(gameModel.missilePrefab, position, Quaternion.identity, transform);
                Enemy_Base enemy = g.GetComponent<Enemy_Base>();
                enemy.SetHitPoint(gameModel.hp_Missile);
                activeEnemies.Add(enemy);

                Behavior_BaddyMissile bm = g.GetComponent<Behavior_BaddyMissile>();
                Vector3 p1 = ServiceLocator.instance.Player.position;
                Vector3 p0 = position;
                Vector3 d0 = -Vector3.forward;
                Vector3 d1 = new Vector3(UnityEngine.Random.Range(-1.00f, 1.00f), UnityEngine.Random.Range(-1.00f, 0.00f), UnityEngine.Random.Range(-1.00f, .25f));
                bm.FireMissile(p0, p1, d0, d1);
            }
        }
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Spawn(EnemyType.SwarmBoys, Vector3.forward * 40, 20);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Spawn(EnemyType.RingDudes, Vector3.forward * 30 + Vector3.down * 20, 20);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Spawn(EnemyType.Mines, Vector3.forward * 30 + ServiceLocator.instance.Player.position, 1);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Spawn(EnemyType.Missiles, Vector3.forward * 30 + ServiceLocator.instance.Player.position, 15);
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

public enum EnemyType { SwarmBoys, RingDudes, Mines, Missiles }
