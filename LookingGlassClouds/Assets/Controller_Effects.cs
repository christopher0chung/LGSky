using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Effects : MonoBehaviour {

    public GameObject scan;
    public float scanTime;
    private float timerScan = 1;

    public GameObject lights;
    private List<GameObject> activeLights;

    void Awake()
    {
        activeLights = new List<GameObject>();
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.U))
            Scan();
        if (Input.GetKeyDown(KeyCode.I))
            Light(Random.Range(-10, 10));
        ScanUpdate();
        LightUpdate();
	}

    public void Scan()
    {
        scanTime = 0;
    }

    private void ScanUpdate()
    {
        scanTime += Time.deltaTime / timerScan;
        scanTime = Mathf.Clamp01(scanTime);
        if (scanTime == 1)
            scan.SetActive(false);
        else
            scan.SetActive(true);
    }

    public void Light(float x)
    {
        Vector3 startPos = new Vector3(x, -5f, -10);
        activeLights.Add(Instantiate<GameObject>(lights, startPos, Quaternion.identity, null));
    }

    private void LightUpdate()
    {
        if (activeLights.Count > 0)
        {
            foreach(GameObject g in activeLights)
            {
                g.transform.position += Vector3.forward * 8 * Time.deltaTime;
                if (g.transform.position.z >= 25)
                {
                    activeLights.Remove(g);
                    Destroy(g);
                }
            }
        }
    }
}
