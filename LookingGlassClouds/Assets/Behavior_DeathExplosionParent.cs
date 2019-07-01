using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_DeathExplosionParent : MonoBehaviour {

    List<ParticleSystem> PSs;

    public void Explode()
    {
        if (PSs == null)
            PSs = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());

        if (PSs != null)
        {
            foreach (ParticleSystem p in PSs)
            {
                p.Play();
            }
        }
    }
}
