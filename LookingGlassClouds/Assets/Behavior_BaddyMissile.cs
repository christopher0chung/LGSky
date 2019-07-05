using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_BaddyMissile : MonoBehaviour {

    private LineRenderer lr;

    private Vector3 position0;
    private Vector3 position1;
    private Vector3 direction0;
    private Vector3 direction1;

    public float firingStrength;
    public float closingStrength;
    public float startDist;

    private bool going;
    private bool tracking;
    private float timer;
    private float flightTime;
    private Vector3 lastPos;
    private float lastSpeed;
    private float secondLastSpeed;
    private Vector3 lastDir;
    private int currentCount;
    private int iter = 20;
    private float overFlightTimer;

    // Use this for initialization
    void Start() {
        if (lr == null)
            lr = GetComponent<LineRenderer>();
    }

    public void FireMissile(Vector3 start, Vector3 stop, Vector3 startDir, Vector3 stopDir)
    {
        position0 = start;
        position1 = stop;
        direction0 = Vector3.Normalize(startDir) * firingStrength;
        direction1 = Vector3.Normalize(stopDir) * closingStrength;

        flightTime = Random.Range(3.00f, 3.25f);
        timer = 0;
        overFlightTimer = 0;

        currentCount = iter + 1;
        lr.positionCount = currentCount;

        lr.SetPosition(0, position1);
        for (int i = iter - 1; i >= 0; i--)
        {
            float grad = ((float)i / iter);
            lr.SetPosition(iter - i, Vector3.Lerp((1 - grad) * Vector3.Lerp(position0, position0 + direction0, grad), (grad) * Vector3.Lerp(position1 - direction1, position1, grad), grad));
        }

        transform.position = position0;
        lastPos = position0;

        going = true;
        tracking = true;
    }

    public void ResetMissile()
    {

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //int i = Random.Range(0, 20);
            //if (i == 0)
            //{
            Vector3 p1 = ServiceLocator.instance.Player.position;
            Vector3 p0 = p1 + Vector3.forward * startDist + Random.insideUnitSphere * 3;
            Vector3 d0 = -Vector3.forward;
            Vector3 d1 = new Vector3(Random.Range(-1.00f, 1.00f), Random.Range(-1.00f, 0.00f), Random.Range(-1.00f, .25f));
            FireMissile(p0, p1, d0, d1);
            //}
        }

        if (going)
        {
            if (tracking)
            {
                timer += Time.deltaTime / flightTime;
                timer = Mathf.Clamp01(timer);

                float easedT = Easings.CubicEaseIn(timer);

                currentCount = (int)((1 - easedT) * iter) + 1;
                lr.positionCount = currentCount;

                position1 = ServiceLocator.instance.Player.position;

                for (int i = iter - 1; i >= 0; i--)
                {
                    float grad = ((float)i / iter);
                    if (iter - i < currentCount)
                        lr.SetPosition(iter - i, Vector3.Lerp((1 - grad) * Vector3.Lerp(position0, position0 + direction0, grad), (grad) * Vector3.Lerp(position1 - direction1, position1, grad), grad));
                }
                lr.SetPosition(0, position1);

                transform.position = Vector3.Lerp((1 - easedT) * Vector3.Lerp(position0, position0 + direction0, easedT), (easedT) * Vector3.Lerp(position1 - direction1, position1, easedT), easedT);

                //PointMissile
                lastDir = Vector3.Normalize(transform.position - lastPos);
                transform.GetChild(0).rotation = Quaternion.LookRotation(lastDir);

                //FlameFromSpeed
                secondLastSpeed = lastSpeed;
                lastSpeed = Vector3.Distance(transform.position, lastPos) / Time.deltaTime;
                transform.GetChild(0).GetChild(0).localScale = Vector3.one * .25f * lastSpeed;

                lastPos = transform.position;

                if (timer >= .95f)
                    tracking = false;
            }
            else
            {
                lr.positionCount = 0;
                transform.position += lastDir * secondLastSpeed * Time.deltaTime;
                overFlightTimer += Time.deltaTime;

                if (overFlightTimer >= 2)
                    going = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.name == "Player")
        {
            Debug.Log("Recognize I hit player");
            GameObject g = ServiceLocator.instance.Controller.GetComponent<Manager_GameAssets>().Make(MyGameAsset.MineExplosion, ServiceLocator.instance.Player.position);
            if (g != null)
                Debug.Log("Something was made... " + g.name);
        }

        if (other.name == "Bullet(Clone)" || other.name == "Rocket(Clone)" || other.name == "Sword")
            Destroy(this.gameObject);
    }
}
