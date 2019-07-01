using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Mine : MonoBehaviour {

    Enemy_Base thisIsMe;

    void Awake()
    {
        thisIsMe = GetComponent<Enemy_Base>();
        SCG_EventManager.instance.Register<Event_EnemyMineHit>(MineHitHandler);
    }

    void MineHitHandler(SCG_Event e)
    {
        Event_EnemyMineHit m = e as Event_EnemyMineHit;
        if (m != null)
        {
            Debug.Assert(m.enemyToBeDestroyed != null, "The enemy class passed to me is null");
            if (m.enemyToBeDestroyed == thisIsMe)
                GetComponent<Enemy_Base>().FireDestructionEvent();
        }
    }

	void Update () {
        if (transform.position.z < -10)
        {
            GetComponent<Enemy_Base>().FireDestructionEvent();
        }
	}
}
