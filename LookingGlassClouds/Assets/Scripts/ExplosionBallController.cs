using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBallController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.localScale = Vector3.zero;
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

        if (timer >= 1.1f)
            Destroy(gameObject);
    }
}
