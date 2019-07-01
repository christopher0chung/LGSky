using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Sword : MonoBehaviour {

    private Model_Game gameModel;
   
    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy")
            return;
        else
        {
            Enemy_Base e = other.gameObject.GetComponent<Enemy_Base>();
            if (e != null)
            {
                Debug.Log("The sword hit " + e.gameObject.name);
                SCG_EventManager.instance.Fire(new Event_PlayerSwordHit(e, gameModel.d_SwordDamage, e.transform.position));
            }
        }
    }
}
