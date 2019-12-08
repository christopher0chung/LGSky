using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_EffectWorldScroll : MonoBehaviour
{
    bool hasFired;
    Model_Play playModel;

    public void Fire(Vector3 forward)
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        transform.rotation = Quaternion.LookRotation(forward);
        GetComponent<ParticleSystem>().Play();
        hasFired = true;
    }

    float timer;

    void Update()
    {
        if (hasFired)
        {
            timer += Time.deltaTime;
            if (timer >= 7)
                Destroy(this.gameObject);

            transform.position -= Vector3.forward * playModel.worldSpeed_Current * Time.deltaTime;
        }
    }
}
