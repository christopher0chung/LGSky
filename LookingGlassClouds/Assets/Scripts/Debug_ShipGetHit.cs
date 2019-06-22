using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_ShipGetHit : MonoBehaviour {

    public GameObject explosion;
    private Manager_GameAssets gAManager;

    void Awake()
    {
        gAManager = ServiceLocator.instance.Controller.GetComponent<Manager_GameAssets>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name== "EnemyBullet(Clone)")
        {
            //Instantiate(explosion, transform.position, Quaternion.identity);
            gAManager.Make(MyGameAsset.BulletExplosion, transform.position);
            SCG_EventManager.instance.Fire(new Event_EnemyBulletHit());
            Destroy(other.gameObject);
        }
    }
}
