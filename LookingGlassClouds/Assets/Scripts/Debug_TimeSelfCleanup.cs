using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_TimeSelfCleanup : MonoBehaviour {

    public float selfCleanUpTime;

    private float timer;

	void Update () {
        timer += Time.deltaTime;
        if (timer >= selfCleanUpTime)
            Destroy(this.gameObject);
	}
}
