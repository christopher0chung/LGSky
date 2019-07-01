using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_RotationSound : MonoBehaviour {

    Model_Game gameModel;
    AudioSource mySource;
    Quaternion lastRot;
    public float delAng;

    private float soundVal;

	// Use this for initialization
	void Start () {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        mySource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        delAng = Quaternion.Angle(Quaternion.LookRotation(gameModel.shieldForwardDirection), lastRot) * Time.deltaTime;
        lastRot = Quaternion.LookRotation(gameModel.shieldForwardDirection);

        if (delAng > soundVal)
            soundVal = Mathf.Clamp01(delAng);
        else
            soundVal = Mathf.Lerp(soundVal, 0, .08f);

        mySource.volume = soundVal;
	}
}
