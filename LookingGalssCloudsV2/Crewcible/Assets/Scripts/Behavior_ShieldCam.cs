using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Behavior_ShieldCam : MonoBehaviour
{
    public GameObject outputQuad;

    Model_Play playModel;

    Vector3 scratch;

    // Start is called before the first frame update
    void Start()
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playModel.unaccessedStations.Contains(Stations.Shield) || playModel.currentPlayerState != PlayerState.Alive)
            outputQuad.SetActive(false);
        else
        {
            outputQuad.SetActive(true);
            transform.rotation = Quaternion.LookRotation((Vector3)playModel.shieldDirection);
            scratch = outputQuad.transform.localPosition;
            if (playModel.leftStation == Stations.Shield)
                scratch.x = -31;
            else
                scratch.x = 31;
            outputQuad.transform.localPosition = scratch;
        }
    }
}
