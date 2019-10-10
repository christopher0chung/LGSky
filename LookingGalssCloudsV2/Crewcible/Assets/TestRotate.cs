using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour
{
    public Vector3 rotationRate;
    void Update()
    {
        transform.Rotate(rotationRate * Time.deltaTime);       
    }
}
