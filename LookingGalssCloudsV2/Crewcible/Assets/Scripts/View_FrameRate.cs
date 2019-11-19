using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_FrameRate : MonoBehaviour
{
    public GameObject frameRate;
    bool visible;

    private void Start()
    {
        visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
            visible = !visible;

        frameRate.SetActive(visible);
    }
}
