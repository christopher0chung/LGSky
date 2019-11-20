using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudTreadmill : MonoBehaviour
{
    List<Transform> clouds;

    public GameObject cloudPrefab;
    public int max;

    public float minZ;
    public float maxZ;
    private float delZ;

    Model_Game gameModel;
    Model_Play playModel;

    void Start()
    {
        delZ = maxZ - minZ;

        clouds = new List<Transform>();
        for (int i = 0; i < max; i++)
        {
            clouds.Add(Instantiate(cloudPrefab, new Vector3(Random.Range(-190, 190), Random.Range(-150f, -60f), Random.Range(minZ, maxZ)), Quaternion.Euler(-90, 0, Random.Range(-180, 180)), transform).transform);
            clouds[i].localScale = Vector3.one * Random.Range(5.00f, 10.00f);
        }

        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
    }

    Vector3 hold;
    void Update()
    {
        foreach(Transform t in clouds)
        {
            t.position += -Vector3.forward * playModel.worldSpeed_Current * Time.deltaTime;
            if (t.position.z <= minZ)
            {
                t.position += Vector3.forward * delZ;
                hold = t.position;
                hold.x = Random.Range(-190, 190);
                t.position = hold;
            }
        }
    }
}
