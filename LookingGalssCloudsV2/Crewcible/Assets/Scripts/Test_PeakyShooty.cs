using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PeakyShooty : MonoBehaviour
{
    Transform target;

    Vector3 oldPos;
    Vector3 newPos;

    public float interval;
    public float bobRate;

    private float _lastTime;
    private float _tOffset;

    Transform shield;

    [Range(0, 1)] public float shieldFraction;
    float _oldShieldT;

    public GameObject bullet;

    private float timer;
    private float lastApproachTimer;
    private float approachTimer;

    Enemy_Base myE;

    // Start is called before the first frame update
    void Start()
    {
        myE = GetComponent<Enemy_Base>();
        target = ServiceLocator.instance.Player;
        oldPos = transform.position;
        newPos = target.position + Vector3.forward * Random.Range(7, 40) + Vector3.right * Random.Range(-33, 33) + Vector3.up * Random.Range(5, 33);
        _tOffset = Random.Range(0.00f, 10.00f);
        timer = -_tOffset;
        shield = transform.GetChild(1);
        SCG_EventManager.instance.Register<Event_EnemyDeath>(EnemyDeathHandler);
    }

    // Update is called once per frame
    void Update()
    {
        approachTimer += Time.deltaTime;

        if (lastApproachTimer <= 7 && approachTimer > 7)
            Tick();
        lastApproachTimer = approachTimer;

        if (approachTimer <= 7)
            Approach();
        else
            OnStation();
    }

    public void EnemyDeathHandler(SCG_Event e)
    {
        Event_EnemyDeath ed = e as Event_EnemyDeath;
        if (ed != null)
        {
            if (ed.enemyToBeDestroyed == myE)
                Destroy(this.gameObject);
        }
    }

    private void Approach()
    {
        transform.position = Vector3.Lerp(oldPos, newPos, Easings.QuinticEaseIn(approachTimer / 7));
        transform.LookAt(target.position);
    }

    private void OnStation()
    {
        timer += Time.deltaTime;
        float t = ((timer + _tOffset) / interval) % 1;

        if (Mathf.Abs(_lastTime - t) > .5f)
            Tick();

        transform.position = Vector3.Lerp(oldPos, newPos, Easings.QuinticEaseInOut(t)) + Vector3.up * 1.3f * Mathf.Sin(timer * bobRate + _tOffset);

        if (t >= shieldFraction)
        {
            float shieldT = (t - shieldFraction) / (1 - shieldFraction);
            shield.localPosition = Vector3.right * 4 * Mathf.Sin(shieldT * Mathf.PI);

            for (int i = 0; i < 2; i++)
            {
                if (_oldShieldT < .5f + (i * .1f) && shieldT >= .5f + (i * .1f))
                    Instantiate(bullet, transform.position + transform.right * -2.65f + transform.forward * 3.07f, Quaternion.LookRotation(transform.forward));
            }
            _oldShieldT = shieldT;
        }

        transform.LookAt(target.position);

        _lastTime = t;
    }
    private void Tick()
    {
        oldPos = newPos;
        newPos = target.position + Vector3.forward * Random.Range(7, 40) + Vector3.right * Random.Range(-33, 33) + Vector3.up * Random.Range(5, 33);
        newPos.y = Mathf.Abs(newPos.y);
    }
}
