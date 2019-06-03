using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_DieOnHit : MonoBehaviour {

    public GameObject explosion;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Bullet(Clone)")
        {
            Instantiate(explosion, transform.position, Quaternion.identity, null);
            GameObject.Find("Application").transform.Find("Controller").GetComponent<Asset_BoidsManager>().DestructionNotice(GetComponent<Asset_Boid>());
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.name == "ExplosionBall(Clone)" || other.gameObject.name == "Sword")
        {
            Instantiate(explosion, transform.position, Quaternion.identity, null);
            GameObject.Find("Application").transform.Find("Controller").GetComponent<Asset_BoidsManager>().DestructionNotice(GetComponent<Asset_Boid>());
            Destroy(this.gameObject);
        }
    }
}
