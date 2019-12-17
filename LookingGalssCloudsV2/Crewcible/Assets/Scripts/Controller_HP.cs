using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_HP : MonoBehaviour
{
    Model_Play playModel;

    float timer;

    void Start()
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        playModel.playerHP = 5;

        SCG_EventManager.instance.Register<Event_EnemyBulletHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_Respawn>(EventHandler);
        SCG_EventManager.instance.Register<Event_Restart>(EventHandler);
    }

    float currentDec;
    float lastDec;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1 && playModel.currentPlayerState == PlayerState.Alive)
        {
            playModel.playerHP += Time.deltaTime / 20;
            playModel.playerHP = Mathf.Clamp(playModel.playerHP, 0, 5);
        }

        currentDec = playModel.playerHP - Mathf.Floor(playModel.playerHP);

        if (currentDec >= .5f && lastDec < .5f)
            SCG_EventManager.instance.Fire(new Event_LifeUpTick());
        if (currentDec < .1f && lastDec > .9f)
            SCG_EventManager.instance.Fire(new Event_LifeUpTick());

        lastDec = currentDec;
    }



    public void EventHandler(SCG_Event e)
    {
        Event_EnemyBulletHit bH = e as Event_EnemyBulletHit;
        if (bH != null)
        {
            float dec = playModel.playerHP - Mathf.Floor(playModel.playerHP);

            if (dec == 0)
                playModel.playerHP--;
            else if (dec >= .5f)
                playModel.playerHP = Mathf.Floor(playModel.playerHP);
            else
            {
                playModel.playerHP = Mathf.Floor(playModel.playerHP);
                playModel.playerHP--;
            }
            playModel.playerHP = Mathf.Clamp(playModel.playerHP, 0, 5);
            timer = 0;
            return;
        }

        Event_Respawn respawn = e as Event_Respawn;
        Event_Restart restart = e as Event_Restart;

        if (respawn != null || restart != null)
        {
            playModel.playerHP = 5;
        }
    }
}
