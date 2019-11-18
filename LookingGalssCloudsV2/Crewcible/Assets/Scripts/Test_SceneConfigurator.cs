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
    

    // Start is called before the first frame update
    void Start()
    {
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
                if (c!= null)
                    c.backgroundColor = happy;
            }
        }
    }
}
