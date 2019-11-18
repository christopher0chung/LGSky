using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View_Lives : MonoBehaviour
{
    public Image[] lives;

    Model_Play playModel;
    void Start()
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (playModel.lives > i)
                lives[i].enabled = true;
            else
                lives[i].enabled = false;
        }
    }
}
