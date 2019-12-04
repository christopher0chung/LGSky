using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_DamageInd : MonoBehaviour
{
    Enemy_Base myE;

    ParticleSystem pS;

    public float maxLife;

    float lastLife;
    void Start()
    {
        myE = GetComponent<Enemy_Base>();
        pS = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (myE.hitpoints_Current != lastLife)
        {
            var emission = pS.emission;
            emission.rateOverTime = Mathf.Lerp(10, 0, myE.hitpoints_Current / maxLife);
        }
        lastLife = myE.hitpoints_Current;
    }
}
