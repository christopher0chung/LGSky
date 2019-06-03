using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_RotationSound : MonoBehaviour {

    AudioSource mySource;

    public Debug_InControlTest d;

    Quaternion lastRot;
    public float delAng;

    private float soundVal;

	// Use this for initialization
	void Start () {
        mySource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        delAng = Quaternion.Angle(Quaternion.LookRotation(d.fwdShield), lastRot) * Time.deltaTime;
        lastRot = Quaternion.LookRotation(d.fwdShield);

        if (delAng > soundVal)
            soundVal = Mathf.Clamp01(delAng);
        else
            soundVal = Mathf.Lerp(soundVal, 0, .08f);

        mySource.volume = soundVal;
	}
}
