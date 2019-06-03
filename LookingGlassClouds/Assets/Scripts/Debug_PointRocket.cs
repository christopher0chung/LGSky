using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_PointRocket : MonoBehaviour {

    Transform model;
    public Vector3 lastPos;
    public Vector3 moveDir;
    private Quaternion lookRotation;

    void Start()
    {
        model = transform.GetChild(0);
    }

	void Update () {
        moveDir = transform.position - lastPos;
        lastPos = transform.position;

        lookRotation = Quaternion.LookRotation(moveDir);

        model.rotation = lookRotation;
	}
}
