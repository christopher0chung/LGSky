using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FPS : MonoBehaviour
{
    Text outputText;
    float delTimeAvg;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        outputText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > 1 && timer <= 1)
            delTimeAvg = Time.unscaledDeltaTime;

        timer += Time.unscaledDeltaTime;


        if (Time.time > 1)
        {
            delTimeAvg = Mathf.Lerp(delTimeAvg, Time.unscaledDeltaTime, .01f);

            outputText.text = (1 / delTimeAvg).ToString();
        }
        else
            outputText.text = "wait...";
    }
}
