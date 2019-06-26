using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_PointAtShieldDir : MonoBehaviour {

    public Model_Game gameModel;

	void Update () {
        transform.rotation = Quaternion.LookRotation(gameModel.shieldForwardDirection);	
	}
}
