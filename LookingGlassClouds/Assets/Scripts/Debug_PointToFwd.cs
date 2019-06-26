using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_PointToFwd : MonoBehaviour {

    public Vector3 flyDir;

    Vector3 lastPos;
	
	void Update () {
        transform.position += flyDir * Time.deltaTime * 5;
        transform.rotation = Quaternion.LookRotation(flyDir);
	}

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Enemy bullet collision!!! " + other.gameObject.name);
    }
}
