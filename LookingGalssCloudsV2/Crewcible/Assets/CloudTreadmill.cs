using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudTreadmill : MonoBehaviour
{
    List<Transform> clouds;

    public GameObject cloudPrefab;
    public int max;
    void Start()
    {
        clouds = new List<Transform>();
        for (int i = 0; i < max; i++)
        {
            clouds.Add(Instantiate(cloudPrefab, new Vector3(Random.Range(-150, 150), Random.Range(-55f, -40f), Random.Range(-75f, 290f)), Quaternion.Euler(-90, 0, Random.Range(-180, 180))).transform);
            clouds[i].localScale = Vector3.one * Random.Range(3.00f, 5.00f);
        }
    }


    void Update()
    {
        foreach(Transform t in clouds)
        {
            t.position += -Vector3.forward * 30 * Time.deltaTime;
            if (t.position.z <= -75f)
                t.position += Vector3.forward * 365;
        }
    }
}
