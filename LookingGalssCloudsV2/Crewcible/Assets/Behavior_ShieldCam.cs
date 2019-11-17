using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Behavior_ShieldCam : MonoBehaviour
{
    public GameObject outputQuad;

    Model_Play playModel;

    // Start is called before the first frame update
    void Start()
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playModel.unaccessedStations.Contains(Stations.Shield))
            outputQuad.SetActive(false);
        else
        {
            outputQuad.SetActive(true);
            transform.rotation = Quaternion.LookRotation((Vector3)playModel.shieldDirection);
        }
    }

    private void OnDisable()
    {
        outputQuad.SetActive(false);
    }
}
