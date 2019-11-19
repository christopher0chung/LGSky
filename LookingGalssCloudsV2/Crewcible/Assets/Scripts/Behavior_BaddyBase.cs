using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_BaddyBase : MonoBehaviour
{
    protected Model_Game gameModel;
    protected Model_Play playModel;
    protected Vector3 _o = Vector3.right; // for offset;
     
    protected virtual void GrabStdRefs()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        _o = Vector3.zero;
    }

    protected virtual Vector3 WorldEffectOffset()
    {
        Debug.Assert(_o != Vector3.right, "Attempting to use AdditionalWorldOffset without GrabStdRefs called first.");
        _o = -Vector3.forward * (gameModel.worldSpeed_fwd - 30) * Time.deltaTime;
        return _o;
    }
}
