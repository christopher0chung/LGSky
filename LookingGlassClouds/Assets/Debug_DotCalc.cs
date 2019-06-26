using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Debug_DotCalc : MonoBehaviour {

    public Vector3 p1;
    public Vector3 p2;

    public Transform t1;
    public Transform t2;

    public float dot;

	void Update () {
        t1.position = transform.position + p1;
        t2.position = transform.position + p2;
        dot = Vector3.Dot(Vector3.Normalize(t1.position - transform.position), Vector3.Normalize(t2.position - transform.position));
	}
}
