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

    // Start is called before the first frame update
    void Start()
    {
        target = ServiceLocator.instance.Player;
        oldPos = transform.position;
        newPos = transform.position;
        _tOffset = Random.Range(0.00f, 10.00f);
        shield = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        float t = (Time.time/interval) % 1;

        if (Mathf.Abs(_lastTime - t) > .5f)
            Tick();

        transform.position = Vector3.Lerp(oldPos, newPos, Easings.QuinticEaseInOut(t)) + Vector3.up * 1.3f * Mathf.Sin(Time.time * bobRate + _tOffset);

        if(t >= shieldFraction)
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
