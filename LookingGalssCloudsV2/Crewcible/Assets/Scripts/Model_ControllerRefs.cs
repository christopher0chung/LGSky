using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Model_ControllerRefs : MonoBehaviour
{
    public InputDevice device0;

    public InputDevice device1;

    void Awake()
    {
        gameObject.tag = "InControl";
        if (GameObject.FindGameObjectsWithTag("InControl").Length > 1)
            Destroy(this.gameObject);
    }
}
