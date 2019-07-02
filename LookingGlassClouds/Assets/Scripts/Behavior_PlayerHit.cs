using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_PlayerHit : MonoBehaviour {

    private Manager_GameAssets gAManager;
    private Model_Play playModel;

    public List<string> namesOfBullets;

    void Awake()
    {
        gAManager = ServiceLocator.instance.Controller.GetComponent<Manager_GameAssets>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (namesOfBullets.Contains(other.name) && playModel.currentPlayerState == PlayerState.Alive)
        {
            gAManager.Make(MyGameAsset.BulletExplosion, transform.position);
            SCG_EventManager.instance.Fire(new Event_EnemyBulletHit());
            Destroy(other.gameObject);
        }
        else if (other.name == "Mine(Clone)")
        {
            Enemy_Base e = null;
            e = other.GetComponent<Enemy_Base>();
            Debug.Assert(e != null, "Mine doesn't have EnemyBase");
            SCG_EventManager.instance.Fire(new Event_EnemyMineHit(e, other.transform.position));
        }
    }
}
