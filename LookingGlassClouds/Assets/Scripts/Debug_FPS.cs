using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_FPS : MonoBehaviour {

    int frameCounter;
    float avgDelTime;

    Text myText;

    private void Start()
    {
        myText = GetComponent<Text>();
    }

	void Update () {
        frameCounter++;
        avgDelTime = Mathf.Lerp(Time.deltaTime, avgDelTime, .95f);

        if (frameCounter >= 10)
        {
            frameCounter = 0;

            myText.text = (1.00f/avgDelTime).ToString();
        }
	}
}
