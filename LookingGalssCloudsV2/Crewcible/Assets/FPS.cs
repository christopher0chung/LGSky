using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FPS : MonoBehaviour
{
    Text outputText;
    float delTimeAvg;

    // Start is called before the first frame update
    void Start()
    {
        outputText = GetComponent<Text>();
        delTimeAvg = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        delTimeAvg = Mathf.Lerp(delTimeAvg, Time.deltaTime, .01f);

        outputText.text = (1 / delTimeAvg).ToString();
    }
}
