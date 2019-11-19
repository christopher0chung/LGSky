using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Bullet : MonoBehaviour {

    Model_Game gameModel;
    Manager_GameAssets assets;

    void Awake()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        assets = ServiceLocator.instance.Controller.GetComponent<Manager_GameAssets>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Blocker")
        {
            GameObject g = assets.Make(MyGameAsset.SFX, transform.position);
            g.GetComponent<AudioSource>().PlayOneShot(gameModel.sfx_EnemyBulletHit);

            SCG_EventManager.instance.Fire(new Event_BonusPoints(1));
            SCG_EventManager.instance.Fire(new Event_PlayerBulletHit(null, 0, transform.position, this));
        }
        else if (other.tag == "Enemy")
        {
            GameObject g = assets.Make(MyGameAsset.SFX, transform.position);
            g.GetComponent<AudioSource>().PlayOneShot(gameModel.sfx_EnemyBulletHit);

            Enemy_Base e = other.gameObject.GetComponent<Enemy_Base>();
            if (e != null)
            {
                //Debug.Log("Hit!");
                SCG_EventManager.instance.Fire(new Event_BonusPoints(2));
                SCG_EventManager.instance.Fire(new Event_PlayerBulletHit(e, gameModel.d_Guns_Damage, transform.position, this));
            }
        }
    }
}
