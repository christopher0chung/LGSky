using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_SwordRotationSound : MonoBehaviour {

    AudioSource mySource;

    Quaternion lastRot;
    public float delAng;

    private float soundVal;

    // Use this for initialization
    void Start()
    {
        mySource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        delAng = Quaternion.Angle(transform.rotation, lastRot) * Time.deltaTime;
        lastRot = transform.rotation;

        if (delAng > soundVal)
            soundVal = Mathf.Clamp01(delAng);
        else
            soundVal = Mathf.Lerp(soundVal, 0, .15f);

        mySource.volume = soundVal / 2;
    }
}
