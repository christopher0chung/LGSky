using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_NoiseBob : MonoBehaviour
{
    public Vector3 bobVector;

    public float bobRate;

    public float rollMin;
    public float rollMax;
    public float rollRate;

    void Update()
    {
        transform.position = bobVector * Mathf.PerlinNoise(0, Time.time * bobRate);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(rollMin, rollMax, Mathf.PerlinNoise(0, Time.time * rollRate)));
    }
}
