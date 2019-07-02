using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBallController : MonoBehaviour {

    Model_Game gameModel;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
    }

	// Use this for initialization
	void OnEnable () {
        transform.localScale = Vector3.zero;
        timer = 0;
        GetComponent<AudioSource>().Play();
	}

    float maxSize = 2;

    private float timer;

	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer < .2f)
        {
            transform.localScale = Vector3.one * (5 * timer * maxSize);
        }
        else if (timer >= .2f && timer < .8f)
            transform.localScale = Vector3.one * maxSize;
        else
            transform.localScale = Vector3.one * Mathf.Lerp(maxSize, 0, (timer - .8f) * 5);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag != "Enemy")
            return;
        else
        {
            Enemy_Base e = other.gameObject.GetComponent<Enemy_Base>();
            if (e != null)
            {
                //Debug.Log("Hit!");
                SCG_EventManager.instance.Fire(new Event_ExplosionBallHit(e, gameModel.d_ExplosionBallDamage * Time.fixedDeltaTime, e.transform.position));
            }
        }
    }
}
