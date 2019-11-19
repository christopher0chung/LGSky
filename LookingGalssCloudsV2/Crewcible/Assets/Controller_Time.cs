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
    }

    void Update()
    {
        if (playModel.currentPlayerState == PlayerState.GameOver || 
            playModel.currentPlayerState == PlayerState.LevelVictory)
        {
            playModel.isPaused = false;
            Time.timeScale = Mathf.Lerp(Time.timeScale, .09f, 2 * Time.unscaledDeltaTime);
        }
        else if (playModel.currentPlayerState == PlayerState.Dash ||
            playModel.currentPlayerState == PlayerState.Flyby)
        {
            playModel.isPaused = false;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, 2 * Time.unscaledDeltaTime);
        }
        else
        {
            if (inputModel.startPause)
                playModel.isPaused = !playModel.isPaused;

            if (playModel.isPaused)
                Time.timeScale = 0;
            else
                Time.timeScale = Mathf.Lerp(Time.timeScale, 1, 2 * Time.unscaledDeltaTime);
        }
    }
}
