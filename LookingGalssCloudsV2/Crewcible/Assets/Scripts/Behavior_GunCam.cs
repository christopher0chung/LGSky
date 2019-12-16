using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_GunCam : MonoBehaviour
{
    public GameObject outputQuad;
    public Transform gunPointer;
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
        if (playModel.unaccessedStations.Contains(Stations.Guns) || playModel.currentPlayerState != PlayerState.Alive)
            outputQuad.SetActive(false);
        else
        {
            outputQuad.SetActive(true);
            transform.rotation = Quaternion.LookRotation(gunPointer.forward);

            scratch = outputQuad.transform.localPosition;
            if (playModel.leftStation == Stations.Guns)
                scratch.x = -31;
            else
                scratch.x = 31;
            outputQuad.transform.localPosition = scratch;
        }
    }
}
