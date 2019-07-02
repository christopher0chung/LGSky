using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_RocketFlight : MonoBehaviour {

    public Manager_GameAssets gameAssets;
    public Model_Game gameModel;
    public Vector3 ultimatePath;
    public Vector3 basePath = Vector3.forward;
    public float rocketSpeed;
    public float windingRadiusMax;
    public float windTime;
    private Vector3 basePos;
    public Vector3 offsetTgt;
    public Vector3 offset;
    private float timer;
    private float windingRadiusTimer;
    private float turnTimer;

    private Vector2 hold;

    public GameObject explosionBall;

    private void OnEnable()
    {
        if (gameAssets == null)
            gameAssets = ServiceLocator.instance.Controller.GetComponent<Manager_GameAssets>();
        if (gameModel == null)
            gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();

        Debug.Assert(gameAssets != null && gameModel != null, " model or manager unhooked");

        basePath = Vector3.forward;
        basePos = transform.position;
        turnTimer = 0;
        timer = 0;
        offset = Vector3.zero;
        offsetTgt = Random.insideUnitSphere * 10;
        windingRadiusTimer = 0;
        GetComponentInChildren<TrailRenderer>().Clear();
        //basePath = Vector3.Normalize(new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f), 1));
    }

	void Update () {

        turnTimer += Time.deltaTime/gameModel.t_RocketTurnTimeNormalized;
        turnTimer = Mathf.Clamp01(turnTimer);

        basePath = Vector3.Lerp(basePath, ultimatePath, Easings.Interpolate(turnTimer, Easings.Functions.ElasticEaseInOut));

        basePos += (basePath) * rocketSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        windingRadiusTimer += Time.deltaTime;

        offset = Vector3.Lerp(offset, offsetTgt, Easings.Interpolate((timer/windTime), Easings.Functions.QuadraticEaseInOut));

        transform.position = Vector3.Lerp(transform.position, basePos + offset, .05f);

        if (timer >= windTime)
        {
            timer -= windTime;
            hold = Random.insideUnitCircle;
            offsetTgt = transform.GetChild(0).rotation * new Vector3(hold.x, hold.y, 0) * windingRadiusMax * windingRadiusTimer;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        Enemy_Base e = other.gameObject.GetComponent<Enemy_Base>();
        if (e != null)
        {
            Debug.Log("Rocket trigger entered");
            SCG_EventManager.instance.Fire(new Event_PlayerRocketHit(e, gameModel.d_RocketDamage, transform.position, this));
        }
    }
}
