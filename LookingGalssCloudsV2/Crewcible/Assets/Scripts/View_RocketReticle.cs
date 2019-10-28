using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_RocketReticle : MonoBehaviour
{
    Model_Input inputModel;
    Model_Play playModel;

    public Transform CW;
    public Transform CCW;
    public Material upDown;

    public MeshRenderer rendCW;
    public MeshRenderer rendCCW;
    public MeshRenderer rendUD;
    void Start()
    {
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playModel.leftStation == Stations.Rockets || playModel.rightStation == Stations.Rockets)
        {
            rendUD.enabled = true;
            if (playModel.leftStation == Stations.Rockets)
                ModelInputs(inputModel.L_X, inputModel.L_Y);
            else
                ModelInputs(inputModel.R_X, inputModel.R_Y);
        }
        else
        {
            rendCCW.enabled = false;
            rendCW.enabled = false;
            rendUD.enabled = false;
        }
    }

    void ModelInputs(float x, float y)
    {
        if (Mathf.Abs(x) > .15f)
        {
            if (x > 0)
            {
                rendCW.enabled = true;
                rendCCW.enabled = false;

                Vector3 rot = CW.rotation.eulerAngles;
                rot.y += x * Time.deltaTime * 180;
                CW.eulerAngles = rot;
            }
            else
            {
                rendCW.enabled = false;
                rendCCW.enabled = true;

                Vector3 rot = CCW.rotation.eulerAngles;
                rot.y += x * Time.deltaTime * 180;
                CCW.eulerAngles = rot;
            }
        }
        else
        {
            rendCW.enabled = false;
            rendCCW.enabled = false;
        }

        upDown.SetFloat("_Arrows", -y);
    }
}
