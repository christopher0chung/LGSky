using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_RocketVis : MonoBehaviour
{
    Transform child;

    public float speed;

    public float xAngTgt;
    public float yAngTgt;

    private float xAng;
    private float yAng;

    Quaternion nextRot;

    float timer;
    float rollover;

    float error = 15;
    float inputError;

    void Start()
    {
        child = transform.GetChild(0);
        nextRot = Quaternion.Euler(Random.Range(-error, error), Random.Range(-error, error), Random.Range(-error, error));
        rollover = Random.Range(.3f, .5f);
        timer = Random.Range(0, rollover);
        inputError = Random.Range(.9f, 1.1f);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) ^ Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.W))
                xAngTgt = 45 * inputError;
            if (Input.GetKey(KeyCode.S))
                xAngTgt = -45 * inputError;
        }
        else
        {
            xAngTgt = 0;
        }
        if (Input.GetKey(KeyCode.A))
            yAngTgt -= 1 * inputError;
        if (Input.GetKey(KeyCode.D))
            yAngTgt += 1 * inputError;

        xAng = Mathf.Lerp(xAng, xAngTgt, .05f);
        yAng = Mathf.Lerp(yAng, yAngTgt, .05f);


        timer += Time.deltaTime;
        if (timer >= rollover)
        {
            timer -= rollover;
            rollover = Random.Range(.3f, .5f);
            nextRot = Quaternion.Euler(Random.Range(-error, error), Random.Range(-error, error), Random.Range(-error, error));
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, nextRot, .05f);

        child.localRotation = Quaternion.Euler(xAng, yAng, 0);

        transform.position += child.forward * Time.deltaTime * speed;
    }
}
