using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Time : MonoBehaviour
{
    Model_Play playModel;
    Model_Input inputModel;

    void Start()
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();

        //SCG_EventManager.instance.Register<Event_PlayerBulletHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_PlayerRocketHit>(EventHandler);
        //SCG_EventManager.instance.Register<Event_LanceHit>(EventHandler);
        SCG_EventManager.instance.Register<Event_EnemyDeath>(EventHandler);
    }

    void Update()
    {
        if (playModel.currentPlayerState == PlayerState.GameOver || 
            playModel.currentPlayerState == PlayerState.LevelVictory)
        {
            playModel.isPaused = false;
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, .09f, 2 * Time.unscaledDeltaTime);
        }
        else if (playModel.currentPlayerState == PlayerState.Dash ||
            playModel.currentPlayerState == PlayerState.Flyby)
        {
            playModel.isPaused = false;
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1, 1.5f * Time.unscaledDeltaTime);
        }
        else
        {
            if (inputModel.startPause && (
                playModel.currentPlayerState == PlayerState.Alive ||
                playModel.currentPlayerState == PlayerState.Dead ||
                playModel.currentPlayerState == PlayerState.Respawning))
                playModel.isPaused = !playModel.isPaused;
            else if (playModel.currentPlayerState == PlayerState.Dash ||
                playModel.currentPlayerState == PlayerState.Flyby ||
                playModel.currentPlayerState == PlayerState.GameOver ||
                playModel.currentPlayerState == PlayerState.LevelVictory)
                playModel.isPaused = false;

            if (playModel.isPaused)
                Time.timeScale = 0;
            else
                Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1, 7 * Time.unscaledDeltaTime);
        }
    }

    public void EventHandler(SCG_Event e)
    {
        if (playModel.currentPlayerState == PlayerState.Alive)
        {
            float timeDip = 1;

            Event_PlayerBulletHit bh = e as Event_PlayerBulletHit;
            if (bh != null)
                timeDip = .5f;

            Event_PlayerRocketHit rh = e as Event_PlayerRocketHit;
            if (rh != null)
                timeDip = .1f;

            Event_LanceHit lh = e as Event_LanceHit;
            if (lh != null)
                timeDip = .65f;

            Event_EnemyDeath ed = e as Event_EnemyDeath;
            if (ed != null)
                timeDip = .06f;

            if (timeDip <= Time.timeScale)
                Time.timeScale = timeDip;
        }
    }
}
