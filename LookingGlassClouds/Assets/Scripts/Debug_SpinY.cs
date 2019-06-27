using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_SpinY : MonoBehaviour {

    public Vector3 rotSpeed;

	void Update () {
        transform.Rotate(rotSpeed * Time.deltaTime);
	}
}
