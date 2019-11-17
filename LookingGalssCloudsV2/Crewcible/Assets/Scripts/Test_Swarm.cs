﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Swarm : MonoBehaviour
{
    public int numberInSwarm;
    public float speed;
    public float interpolateFactor;

    public List<GameObject> _swarm;
    private List<BoidDirs> _dirs;
    private List<bool> _attackMode;

    private int indexIterator;

    private Transform target;

    [Header("Tuning Weights")]
    [Range(0, 1)] public float w_Cohesion;
    [Range(0, 1)] public float w_Alignment;
    [Range(0, 1)] public float w_Random;
    [Range(0, 1)] public float w_Target;

    public float maskingRange;
    public float rangeToTargetWeightFactor;
    public float attackPulloutRange;
    [Range(0, 1)] public float attackConeScalar;
    [Range(0, 1)] public float attackPercentage;

    public GameObject bullet;

    float attackTimer;

    void Start()
    {
        SCG_EventManager.instance.Register<Event_EnemyDeath>(EnemyDeathEventHandler);
        Debug.Log("Start Stuff");

        target = ServiceLocator.instance.Player;

        _swarm = new List<GameObject>();
        _dirs = new List<BoidDirs>();
        _attackMode = new List<bool>();

        GameObject first = transform.GetChild(0).gameObject;
        _swarm.Add(first);
        for (int i = 0; i < numberInSwarm - 1; i++)
        {
            _swarm.Add(Instantiate(first, transform.position, Quaternion.LookRotation(Random.insideUnitSphere), transform));
            _dirs.Add(new BoidDirs());
            _attackMode.Add(false);

            if (i == 0)
            {
                _dirs.Add(new BoidDirs());
                _attackMode.Add(false);
            }
        }

        foreach (GameObject g in _swarm)
        {
            g.GetComponent<Enemy_Base>().SetHitPoint(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //For the indexed boid, calculate params
        //For each boid interpolate towards the next direction
        //Push each boid foward
        for (int i = 0; i < 15; i++)
        {
            indexIterator++;
            if (indexIterator >= _dirs.Count)
                indexIterator = 0;

            if (_swarm.Count <= 0)
                Destroy(this.gameObject);
            else
                UpdateSwarmerByIndex(indexIterator);
        }

        CalculateBoidsNewDir();

        attackTimer += Time.deltaTime;
        if (attackTimer >=5 )
        {
            attackTimer -= 5;
            for (int i = 0; i < _swarm.Count / 4; i++)
            {
                _attackMode[Random.Range(0, _swarm.Count)] = true;
            }
        }
    }

    #region Major Functions
    private void InitializeDirections(BoidDirs d)
    {
        d.cohesion = Vector3.zero;
        d.alignment = Vector3.zero;
        d.random = Vector3.Normalize(Random.insideUnitSphere);
    }
    
    private class BoidDirs
    {
        public Vector3 cohesion;
        public Vector3 alignment;
        public Vector3 random;

        public Vector3 nextDir;
    }

    private void UpdateSwarmerByIndex(int indexIterator)
    {
        InitializeDirections(_dirs[indexIterator]);

        Vector3 middle = Vector3.zero;

        int averager = 0;

        foreach (GameObject g in _swarm)
        {
            float dist = Vector3.Distance(g.transform.position, _swarm[indexIterator].transform.position);

            if (dist != 0 && dist <= maskingRange)
            {
                middle += g.transform.position;
                averager++;
                _dirs[indexIterator].alignment += g.transform.forward;
            }
        }
        _dirs[indexIterator].cohesion = Vector3.Normalize(middle / averager - _swarm[indexIterator].transform.position);
        _dirs[indexIterator].alignment = Vector3.Normalize(_dirs[indexIterator].alignment);
    }
    #endregion

    #region Events
    public void EnemyDeathEventHandler (SCG_Event e)
    {
        //Debug.Log("Message Received");

        Event_EnemyDeath d = e as Event_EnemyDeath;

        if (d != null)
        {
            if (_swarm.Contains(d.enemyToBeDestroyed.gameObject))
            {
                int indexToRemove = _swarm.IndexOf(d.enemyToBeDestroyed.gameObject);
                _swarm.RemoveAt(indexToRemove);
                _dirs.RemoveAt(indexToRemove);
                _attackMode.RemoveAt(indexToRemove);
                Destroy(d.enemyToBeDestroyed.gameObject);
            }
        }
    }
    #endregion

    #region Internal
    Vector3 tempTowardsTarget;
    float distToTarget;
    private void CalculateBoidsNewDir()
    {
        for (int i = 0; i < _swarm.Count; i++)
        {
            if (!_attackMode[i])
                _Flocking(i);
            else
                _Attacking(i);
            _ApplyNewDir(i);
        }
    }

    private void _Flocking(int i)
    {
        tempTowardsTarget = Vector3.Normalize(target.transform.position - _swarm[i].transform.position);
        distToTarget = Vector3.Distance(target.transform.position, _swarm[i].transform.position);

        _dirs[i].nextDir = Vector3.Normalize(w_Cohesion * _dirs[i].cohesion +
            w_Alignment * _dirs[i].alignment +
            w_Random * _dirs[i].random +
            w_Target * tempTowardsTarget * distToTarget / rangeToTargetWeightFactor);
    }

    Vector3 targetBrg;
    private void _Attacking(int i)
    {
        _dirs[i].nextDir = Vector3.Normalize(target.transform.position - _swarm[i].transform.position);

        targetBrg = Vector3.Normalize(target.transform.position - _swarm[i].transform.position);

        if (Vector3.Dot(_swarm[i].transform.forward, targetBrg) >= attackConeScalar)
        {
            if (Random.Range(0.00f, 1.00f) <= attackPercentage * Time.deltaTime)
                Instantiate(bullet, _swarm[i].transform.position, Quaternion.LookRotation(targetBrg));
        }

        if (Vector3.Distance(target.position, _swarm[i].transform.position) <= attackPulloutRange)
            _attackMode[i] = false;
    }

    private void _ApplyNewDir(int i)
    {
        _swarm[i].transform.rotation =
            Quaternion.Slerp(
                _swarm[i].transform.rotation,
                Quaternion.LookRotation(_dirs[i].nextDir),
                interpolateFactor * Time.deltaTime);
        _swarm[i].transform.position += _swarm[i].transform.forward * speed * Time.deltaTime;
    }
    #endregion
}