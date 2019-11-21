using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_EnemyFlameout : MonoBehaviour
{
    ParticleSystem myPS;
    Model_Play playModel;
    void Start()
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        myPS = GetComponent<ParticleSystem>();
        myPS.Play();
    }

    float timer;

    private void Update()
    {
        transform.position -= Vector3.forward * playModel.worldSpeed_Current * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= 1.6f)
        {
            myPS.Stop();
            Destroy(this.gameObject, 1.5f);
        }
    }


}
