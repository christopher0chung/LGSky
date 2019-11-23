using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SceneConfigurator : MonoBehaviour
{
    private View_ColorConfigurator cc;
    public bool isNormal;

    public GameObject normalClouds;
    public GameObject happyClouds;

    public GameObject[] objToDisable;
    

    // Start is called before the first frame update
    void Awake()
    {
        cc = ServiceLocator.instance.View.GetComponent<View_ColorConfigurator>();

        Time.timeScale = 1;

        if (isNormal)
        {
            Instantiate(normalClouds, Vector3.zero, Quaternion.identity, ServiceLocator.instance.View);
        }
        else
        {
            Instantiate(happyClouds, Vector3.zero, Quaternion.identity, ServiceLocator.instance.View);

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

    private void Start()
    {
        if (isNormal)
            cc.SetColors(AestheticMode.Normal);
        else
            cc.SetColors(AestheticMode.Happy);
    }
}
