using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_SwordPivot : MonoBehaviour {

    public Transform toFollow;
	void Update () {
        transform.rotation = Quaternion.Slerp(transform.rotation, toFollow.rotation, .35f);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, toFollow.rotation, 1200 * Time.deltaTime);
	}
}
