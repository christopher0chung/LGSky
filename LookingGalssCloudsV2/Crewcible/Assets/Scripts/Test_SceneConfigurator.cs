using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SceneConfigurator : MonoBehaviour
{
    public bool isNormal;
    public Color normal;
    public Color happy;
    public LookingGlass.Holoplay hP;

    public GameObject normalClouds;
    public GameObject happyClouds;

    public Camera[] miniCams;

    public GameObject[] objToDisable;
    

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1;

        if (isNormal)
        {
            hP.background = normal;
            RenderSettings.fogColor = normal;
            Instantiate(normalClouds, Vector3.zero, Quaternion.identity, ServiceLocator.instance.View);
            foreach (Camera c in miniCams)
            {
                if (c!= null)
                    c.backgroundColor = normal;
            }
        }
        else
        {
            hP.background = happy;
            RenderSettings.fogColor = happy;
            Instantiate(happyClouds, Vector3.zero, Quaternion.identity, ServiceLocator.instance.View);
            foreach (Camera c in miniCams)
            {
                if (c != null)
                    c.backgroundColor = happy;
            }
            foreach (GameObject g in objToDisable)
            {
                if (g != null)
                    g.SetActive(false);
            }
            if (ServiceLocator.instance.View.GetComponent<View_Lives>() != null)
                ServiceLocator.instance.View.GetComponent<View_Lives>().enabled = false;
            if (ServiceLocator.instance.View.GetComponent<View_Score>() != null)
                ServiceLocator.instance.View.GetComponent<View_Score>().enabled = false;
            if (ServiceLocator.instance.Controller.GetComponent<Controller_EnemySpawner>() != null)
                ServiceLocator.instance.Controller.GetComponent<Controller_EnemySpawner>().enabled = false;
            if (ServiceLocator.instance.Controller.GetComponent<Controller_Respawn>() != null)
                ServiceLocator.instance.Controller.GetComponent<Controller_Respawn>().enabled = false;
        }
    }
}
