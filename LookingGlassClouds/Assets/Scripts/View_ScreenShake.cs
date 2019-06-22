using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_ScreenShake : MonoBehaviour {

    public Transform cameraXfm;

    public int temp_ShakeCount;
    public float temp_ShakeTime;

    public float timer;
    public float shakeInterval;
    public int shakeCounter;
    public float maxShakeMag;

    public float shakeMagDec;

    private bool _s;
    private bool shakeInitiate
    {
        get { return _s; }
        set
        {
            _s = value;

            if (value)
            {
                shakeInterval = temp_ShakeTime / temp_ShakeCount;
                offsetEuler = new Vector3(Random.Range(-maxShakeMag, maxShakeMag), Random.Range(-maxShakeMag, maxShakeMag), 0);
                timer = 0;
                shakeCounter = 0;
            }
            else
            {
                timer = 0;
                shakeCounter = 0;
            }
        }
    }

    private Vector3 originalEuler;
    public Vector3 offsetEuler;

    void Awake()
    {
        SCG_EventManager.instance.Register<Event_EnemyBulletHit>(PlayerHitEventHandler);
    }

    void Start()
    {
        originalEuler = cameraXfm.localEulerAngles;
    }

    void OnDisable()
    {
        cameraXfm.localEulerAngles = originalEuler;
    }

    void PlayerHitEventHandler(SCG_Event e)
    {
        Event_EnemyBulletHit bH = e as Event_EnemyBulletHit;

        if (bH != null)
            shakeInitiate = true;
    }

	void Update () {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    shakeInitiate = true;

        if (shakeInitiate)
            Shake();
        else
            cameraXfm.localRotation = Quaternion.Slerp(cameraXfm.localRotation, Quaternion.Euler(originalEuler), .08f);

    }

    private void Shake()
    {
        timer += Time.deltaTime;

        cameraXfm.localRotation = Quaternion.Slerp(cameraXfm.localRotation, Quaternion.Euler(originalEuler + offsetEuler), Easings.CubicEaseInOut(timer / shakeInterval));

        if (timer / shakeInterval >= 1)
        {
            timer = 0;
            shakeCounter++;
            shakeMagDec = (float)(temp_ShakeCount - shakeCounter) / (float)temp_ShakeCount;
            offsetEuler = new Vector3(Random.Range(-maxShakeMag, maxShakeMag) * shakeMagDec, Random.Range(-maxShakeMag, maxShakeMag) / 2 * shakeMagDec, 0);
            //Debug.Log(offsetEuler);
        }

        if (shakeCounter >= temp_ShakeCount)
        {
            shakeInitiate = false;
        }
    }
}
