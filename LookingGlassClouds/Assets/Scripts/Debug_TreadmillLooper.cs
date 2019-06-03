using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_TreadmillLooper : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Time.deltaTime * 10 * -Vector3.forward;

        if (transform.position.z <= -13)
        {
            transform.position = new Vector3(Random.Range(-20, 20), Random.Range(0, 4), 45+Random.Range(0,12));
            transform.localScale = new Vector3(Random.Range(5, 12), Random.Range(5, 12), Random.Range(5, 12));
        }
	}
}
