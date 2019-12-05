using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller_PauseMenu : MonoBehaviour
{
    Model_Play playModel;
    Model_Input inputModel;

    public Text pause;
    public Text returnToMenu;
    public Text controls;
    public RectTransform selector;

    private int choices;
    private bool lastPause;

    float threshold = .25f;
    float oldL;
    float oldR;
    // Start is called before the first frame update

    Vector3 tgtPos;
    Vector3 currentPos;
    Vector3 newPos;
    void Start()
    {
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();

        currentPos = selector.transform.position;
        CalcNewPos(pause.transform);
        selector.position = newPos;

        choices = 0;

        pause.enabled = false;
        returnToMenu.enabled = false;
        controls.enabled = false;
        selector.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playModel.isPaused && !lastPause)
        {
            choices = 0;
            pause.enabled = true;
            returnToMenu.enabled = true;
            controls.enabled = true;
            selector.gameObject.SetActive(true);

            CalcNewPos(pause.transform);
            selector.position = newPos;

            oldL = oldR = 0;
        }
        else if (!playModel.isPaused && lastPause)
        {
            pause.enabled = false;
            returnToMenu.enabled = false;
            controls.enabled = false;
            selector.gameObject.SetActive(false);
        }

        lastPause = playModel.isPaused;

        if (playModel.isPaused)
        {
            if ((inputModel.L_Y >= threshold && oldL < threshold) || (inputModel.R_Y >= threshold && oldR < threshold))
            {
                choices--;
                if (choices < 0)
                    choices = 2;
            }
            if ((inputModel.L_Y <= -threshold && oldL > -threshold) || (inputModel.R_Y <= -threshold && oldR > -threshold))
            {
                choices++;
                if (choices > 2)
                    choices = 0;
            }
            oldL = inputModel.L_Y;
            oldR = inputModel.R_Y;

            if (choices == 0)
                CalcNewPos(pause.transform);
            else if(choices == 1)
                CalcNewPos(returnToMenu.transform);
            else if(choices == 2)
                CalcNewPos(controls.transform);

            selector.transform.position = Vector3.Lerp(selector.transform.position, newPos, 8 * Time.unscaledDeltaTime);

            if (inputModel.L_Action_OnDown || inputModel.R_Action_OnDown)
            {
                if (choices == 0)
                    playModel.isPaused = false;
                else if (choices == 1)
                {
                    SCG_EventManager.instance.Clear();
                    SceneManager.LoadScene(0);
                }
                else if (choices == 2)
                {
                    //Do nothing
                }
            }
        }
    }

    void CalcNewPos(Transform t)
    {
        tgtPos = t.transform.position;
        newPos = currentPos;
        newPos.y = tgtPos.y + 70;
    }
}
