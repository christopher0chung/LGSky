using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Swarm : MonoBehaviour
{
    public int numberInSwarm;
    public float speed;

    private List<GameObject> _swarm;
    private List<BoidDirs> _dirs;

    private int indexIterator;

    private Transform target;

    [Header("Tuning Weights")]
    [Range(0, 1)] public float w_Cohesion;
    [Range(0, 1)] public float w_Alignment;
    [Range(0, 1)] public float w_Random;
    [Range(0, 1)] public float w_Target;

    void Start()
    {
        target = ServiceLocator.instance.Player;

        _swarm = new List<GameObject>();

        GameObject first = transform.GetChild(0).gameObject;
        _swarm.Add(first);
        for (int i = 0; i < numberInSwarm - 1; i++)
        {
            _swarm.Add(Instantiate(first, transform.position, Quaternion.LookRotation(Random.insideUnitSphere)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _swarm.Count; i++)
        {
            _swarm[i].transform.position +=
                ((w_Cohesion * _dirs[i].cohesion) +
                (w_Alignment * _dirs[i].alignment) +
                (w_Random * _dirs[i].random) +
                (w_Target * Vector3.Normalize(target.position - _swarm[i].transform.position)))
                * speed * Time.deltaTime;
        }
        indexIterator++;
    }

    private class BoidDirs
    {
        public Vector3 cohesion;
        public Vector3 alignment;
        public Vector3 random;
    }
}
