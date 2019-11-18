using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Bullet : MonoBehaviour {

    Model_Game gameModel;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Blocker")
        {
            SCG_EventManager.instance.Fire(new Event_PlayerBulletHit(null, 0, transform.position, this));
        }
        else if (other.tag == "Enemy")
        {
            Enemy_Base e = other.gameObject.GetComponent<Enemy_Base>();
            if (e != null)
            {
                //Debug.Log("Hit!");
                SCG_EventManager.instance.Fire(new Event_PlayerBulletHit(e, gameModel.d_Guns_Damage, transform.position, this));
            }
        }
    }
}
