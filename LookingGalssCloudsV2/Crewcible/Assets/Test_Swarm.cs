using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Swarm : MonoBehaviour
{
    public int numberInSwarm;
    public float speed;
    public float interpolateFactor;

    private List<GameObject> _swarm;
    private List<BoidDirs> _dirs;

    private int indexIterator;

    private Transform target;

    [Header("Tuning Weights")]
    [Range(0, 1)] public float w_Cohesion;
    [Range(0, 1)] public float w_Alignment;
    [Range(0, 1)] public float w_Random;
    [Range(0, 1)] public float w_Target;

    public float maskingRange;

    void Start()
    {
        target = ServiceLocator.instance.Player;

        _swarm = new List<GameObject>();
        _dirs = new List<BoidDirs>();

        GameObject first = transform.GetChild(0).gameObject;
        _swarm.Add(first);
        for (int i = 0; i < numberInSwarm - 1; i++)
        {
            _swarm.Add(Instantiate(first, transform.position, Quaternion.LookRotation(Random.insideUnitSphere)));
            _dirs.Add(new BoidDirs());

            if (i == 0)
                _dirs.Add(new BoidDirs());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //For the indexed boid, calculate params
        //For each boid interpolate towards the next direction
        //Push each boid foward

        if (indexIterator >= _dirs.Count)
            indexIterator = 0;

        InitializeDirections(_dirs[indexIterator]);

        Vector3 middle = Vector3.zero;
        int averager = 0;

        foreach(GameObject g in _swarm)
        {
            float dist = Vector3.Distance(g.transform.position, _swarm[indexIterator].transform.position);

            if (dist != 0 && dist <= maskingRange)
            {
                middle += g.transform.position;
                averager++;
                _dirs[indexIterator].alignment += g.transform.forward;
            }
        }
        _dirs[indexIterator].cohesion += Vector3.Normalize(middle/averager - _swarm[indexIterator].transform.position);
        _dirs[indexIterator].alignment += Vector3.Normalize(_dirs[indexIterator].alignment);

        Vector3 tempTowardsTarget;
        float distToTarget;
        for (int i = 0; i < _swarm.Count; i++)
        {
            tempTowardsTarget = Vector3.Normalize(target.transform.position - _swarm[i].transform.position);
            distToTarget = Vector3.Distance(target.transform.position, _swarm[i].transform.position);

            _dirs[i].nextDir = Vector3.Normalize(w_Cohesion * _dirs[i].cohesion +
                w_Alignment * _dirs[i].alignment +
                w_Random * _dirs[i].random +
                w_Target * tempTowardsTarget * distToTarget / 50);

            _swarm[i].transform.rotation = 
                Quaternion.Slerp(
                    _swarm[i].transform.rotation, 
                    Quaternion.LookRotation(_dirs[i].nextDir), 
                    interpolateFactor * Time.deltaTime);
            _swarm[i].transform.position += _swarm[i].transform.forward * speed * Time.deltaTime;
        }
        indexIterator++;
    }

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
}
