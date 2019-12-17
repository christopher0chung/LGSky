using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Behavior_ImageBlink : MonoBehaviour
{
    Image myImage;
    float timer;

    public AudioClip boop;

    public Color blinkColor;

    void Start()
    {
        myImage = GetComponent<Image>();
        Debug.Log("Start was called");
        Debug.Assert(myImage != null, "No image present. Nothing to blink.");
    }

    float old = 10;
    void LateUpdate()
    {
        Debug.Log("In update");

        timer += Time.deltaTime;
        blinkColor.a = .5f * Mathf.Cos(timer * 8) + .5f;

        float value = (timer * 8) % (Mathf.PI * 2);

        if (value < .5f && old > 1)
            ServiceLocator.instance.SFX.PlayOneShot(boop, .1f);

        old = value;

        //Debug.Log(blinkColor.a);

        myImage.color = blinkColor;
    }
}
