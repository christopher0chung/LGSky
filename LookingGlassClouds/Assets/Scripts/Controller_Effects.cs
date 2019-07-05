using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Effects : MonoBehaviour {

    public GameObject scan;
    public float scanTime;
    private float timerScan = 1;

    public GameObject normalLight;
    public GameObject spinningLight;
    private List<GameObject> activeLights;
    private List<GameObject> lightsToDestroy;

    void Awake()
    {
        activeLights = new List<GameObject>();
        lightsToDestroy = new List<GameObject>();
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.U))
            Scan();
        if (Input.GetKeyDown(KeyCode.I))
            Light(Random.Range(-10, 10));
        if (Input.GetKeyDown(KeyCode.O))
            SpinningLight();

        _ScanUpdate();
        _LightUpdate();
	}

    public void Scan()
    {
        timerScan = 0;
    }

    private void _ScanUpdate()
    {
        timerScan += Time.deltaTime / scanTime;
        timerScan = Mathf.Clamp01(timerScan);
        if (timerScan == 1)
            scan.SetActive(false);
        else
            scan.SetActive(true);
    }

    public void Light(float x)
    {
        Vector3 startPos = new Vector3(x, -5f, -10);
        activeLights.Add(Instantiate<GameObject>(normalLight, startPos, Quaternion.identity, null));
    }

    public void SpinningLight()
    {
        Vector3 startPos = new Vector3(0, -5f, -10);
        activeLights.Add(Instantiate<GameObject>(spinningLight, startPos, Quaternion.identity, null));
    }

    private void _LightUpdate()
    {
        if (lightsToDestroy.Count > 0)
        {
            for (int i = lightsToDestroy.Count - 1; i >=0; i--)
            {
                GameObject g = lightsToDestroy[i]; 
                if (activeLights.Contains(g))
                    activeLights.Remove(g);
                lightsToDestroy.Remove(g);
                Destroy(g);
            }
        }
        if (activeLights.Count > 0)
        {
            foreach(GameObject g in activeLights)
            {
                g.transform.position += Vector3.forward * 8 * Time.deltaTime;
                if (g.transform.position.z >= 25)
                {
                    lightsToDestroy.Add(g);
                }
            }
        }
    }
}
