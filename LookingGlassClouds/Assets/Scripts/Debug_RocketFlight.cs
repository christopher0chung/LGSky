using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_RocketFlight : MonoBehaviour {

    public Vector3 ultimatePath;
    public Vector3 basePath = Vector3.forward;
    public float rocketSpeed;
    public float windingRadiusMax;
    public float windTime;
    private Vector3 basePos;
    private Vector3 offsetTgt;
    private Vector3 offset;
    private float timer;
    private float cleanUpTimer;
    private float turnTimer;

    private Vector2 hold;

    public GameObject explosionBall;

    private void Start()
    {
        basePos = transform.position;
        //basePath = Vector3.Normalize(new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f), 1));
    }

	void Update () {

        turnTimer += Time.deltaTime/1;
        turnTimer = Mathf.Clamp01(turnTimer);

        basePath = Vector3.Lerp(basePath, ultimatePath, Easings.QuinticEaseIn(turnTimer));

        basePos += (basePath) * rocketSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        offset = Vector3.Lerp(offset, offsetTgt, Easings.Interpolate((timer/windTime), Easings.Functions.QuadraticEaseInOut));

        transform.position = Vector3.Lerp(transform.position, basePos + offset, .05f);

        if (timer >= windTime)
        {
            timer -= windTime;
            hold = Random.insideUnitCircle;
            offsetTgt = new Vector3(hold.x, hold.y, 0) * windingRadiusMax * cleanUpTimer;
        }

        cleanUpTimer += Time.deltaTime;
        if (cleanUpTimer >= 10)
            Destroy(this.gameObject);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Boid(Clone)")
        {
            Instantiate(explosionBall, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
