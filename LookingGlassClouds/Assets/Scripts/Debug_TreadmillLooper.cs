using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_TreadmillLooper : MonoBehaviour {

    Model_Game gameModel;
    public float moveSpeedScalar = 1;
	// Use this for initialization
	void Start () {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Time.deltaTime * gameModel.worldSpeed_fwd * -Vector3.forward * moveSpeedScalar;

        //if (transform.position.z <= -13)
        //{
        //    transform.position = new Vector3(Random.Range(-20, 20), Random.Range(0, 4), 45+Random.Range(0,12));
        //    transform.localScale = new Vector3(Random.Range(5, 12), Random.Range(5, 12), Random.Range(5, 12));
        //}
	}
}
