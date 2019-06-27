using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_PilotJumpParticles : MonoBehaviour {

    List<ParticleSystem> jumpParticles;

    void Start()
    {
        jumpParticles = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());
    }

    public void Play(Vector3 jumpStartPos, Vector3 fwdDir)
    {
        Debug.Log(fwdDir);

        transform.position = jumpStartPos;
        transform.rotation = Quaternion.LookRotation(fwdDir);
        foreach (ParticleSystem p in jumpParticles)
            p.Play();
    }
}
