using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Strafer : MonoBehaviour
{
    public Transform turret;

    private Transform target;

    private Vector3 projection;

    private float shootTimer;
    public GameObject bullet;
    void Start()
    {
        target = ServiceLocator.instance.Player;
    }

    void Update()
    {
        projection = target.position;
        projection.y = turret.position.y;
        turret.rotation = Quaternion.LookRotation(projection - turret.position, transform.up);
        transform.position += (transform.forward * 37 + Vector3.forward * -30) * Time.deltaTime;

        shootTimer += Time.deltaTime;
        if (shootTimer >= 2)
        {
            shootTimer = 0;
            if (turret.position.z > target.position.z + 12 && turret.position.z < 100)
                Instantiate(bullet, turret.position + turret.forward * 1.1f, Quaternion.LookRotation(target.position - turret.position, transform.up));
        }

        if (transform.position.z >= 270)
            Destroy(this.gameObject);
    }
}
