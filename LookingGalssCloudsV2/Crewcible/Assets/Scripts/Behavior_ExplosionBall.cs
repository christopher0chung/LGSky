using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_ExplosionBall : MonoBehaviour
{
    Model_Game gameModel;
    private float timer;

    private float quarterLife;

    private void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();

        quarterLife = gameModel.t_Rockets_ExplosionBallLifetime * .25f;
    }
    void Update()
    {
        transform.position -= Vector3.forward * Time.deltaTime * gameModel.worldSpeed_fwd;

        timer += Time.deltaTime;
        if (timer <= quarterLife)
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * gameModel.f_Rockets_ExplosionBallSize, Easings.BounceEaseInOut(timer / quarterLife));
        else if (timer >= quarterLife * 3)
            transform.localScale = Vector3.Lerp(Vector3.one * gameModel.f_Rockets_ExplosionBallSize, Vector3.zero, Easings.BounceEaseInOut((timer - (3 * quarterLife)) / quarterLife));
    }

    private void OnEnable()
    {
        //Debug.Log("OnEnable is working");
        transform.localScale = Vector3.zero;
        timer = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
            return;

        Debug.Log("enemy in radius");

        Enemy_Base e = other.GetComponent<Enemy_Base>();

        if (e != null)
            SCG_EventManager.instance.Fire(new Event_ExplosionBallHit(e, gameModel.d_Rockets_ExplosionBallDamage, other.transform.position));
    }
}
