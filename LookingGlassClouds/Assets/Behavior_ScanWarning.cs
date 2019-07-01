using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_ScanWarning : MonoBehaviour {

    //private LineRenderer _lr;
    private Vector3 scanPos;

    public float yMag;
    public float yPd;

    public float yOffset;
    public float tOffset;

    private float time;

	// Use this for initialization
	void Start () {
        //_lr = GetComponent<LineRenderer>();
        tOffset = Random.Range(0, 30);
	}
	
	// Update is called once per frame
	void Update () {
        time = Time.time + tOffset;

        scanPos.y = Mathf.Sin(time * yPd) * yMag + yOffset;
        //_lr.SetPosition(1, scanPos);

        transform.rotation = Quaternion.Euler(scanPos);
	}
}
