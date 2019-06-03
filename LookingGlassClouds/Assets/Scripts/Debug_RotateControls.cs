using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_RotateControls : MonoBehaviour {

	void Update () {
        if (Input.GetKey(KeyCode.I))
            transform.localRotation *= Quaternion.Euler(50 * Time.deltaTime, 0, 0);
        if (Input.GetKey(KeyCode.K))
            transform.localRotation *= Quaternion.Euler(-50 * Time.deltaTime, 0, 0);
        if (Input.GetKey(KeyCode.L))
            transform.localRotation *= Quaternion.Euler(0, 50 * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.J))
            transform.localRotation *= Quaternion.Euler(0, -50 * Time.deltaTime, 0);
    }
}
