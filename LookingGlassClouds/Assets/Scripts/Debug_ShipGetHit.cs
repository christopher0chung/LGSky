using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_ShipGetHit : MonoBehaviour {

    public GameObject explosion;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name== "EnemyBullet(Clone)")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }
}
