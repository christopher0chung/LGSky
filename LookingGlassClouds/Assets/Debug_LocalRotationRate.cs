using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_LocalRotationRate : MonoBehaviour {

    public Vector3 localRotationRate;

	void Update () {
        transform.localEulerAngles += localRotationRate * Time.deltaTime;		
	}
}
