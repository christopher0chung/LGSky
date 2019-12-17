using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller_Menu : SCG_Controller
{
    Model_ControllerRefs input;
    Model_Input inputModel;

    public Text p1;
    public Text p2;
    public Text c;

    private float oldL;
    private float oldR;

    public Image up;
    public Image down;
    public Image aButton;
    public Image rightTrigger;

    int choices;
    float threshold = .25f;

    Color normalArrowColor = new Color(1, 1, 1, .25f);

    Color buttonColor = Color.white;

    private void Start()
    {
        priority = 2;
        Schedule(this);
        input = ServiceLocator.instance.controllerRefs;
        inputModel = ServiceLocator.instance.Model.GetComponent<Model_Input>();
        choices = 0;
    }

    public override void ScheduledUpdate()
    {
        buttonColor.a = .5f * Mathf.Sin(7 * Time.time) + .5f;
        aButton.color = rightTrigger.color = buttonColor;

        if (input.device0 != null)
            p1.text = "P1";
        else
            p1.text = "";

        if (input.device1 != null) 
            p2.text = "P2";
        else
            p2.text = "";

        if ((inputModel.L_Y >= threshold && oldL < threshold) || (inputModel.R_Y >= threshold && oldR < threshold))
        {
            choices--;
            if (choices < 0)
                choices = 2;
            up.color = Color.white;
        }
        if ((inputModel.L_Y <= -threshold && oldL > -threshold) || (inputModel.R_Y <= -threshold && oldR > -threshold))
        {
            choices++;
            if (choices > 2)
                choices = 0;
            down.color = Color.white;
        }

        up.color = Color.Lerp(up.color, normalArrowColor, 5 * Time.deltaTime);
        down.color = Color.Lerp(down.color, normalArrowColor, 5 * Time.deltaTime);

        oldL = inputModel.L_Y;
        oldR = inputModel.R_Y;

        if (choices == 0)
            c.text = "PLAY";
        else if (choices == 1)
            c.text = "INTRO";
        else if (choices == 2)
            c.text = "PRACTICE";

        if ((inputModel.acknowledge) && (ServiceLocator.instance.controllerRefs.device0 != null || ServiceLocator.instance.controllerRefs.device1 != null))
        {
            if (choices == 0)
            {
                SCG_EventManager.instance.Clear();
                SceneManager.LoadScene(1);
            }
            else if (choices == 1)
            {
                SCG_EventManager.instance.Clear();
                SceneManager.LoadScene(2);
            }
            else if (choices == 2)
            {
                SCG_EventManager.instance.Clear();
                SceneManager.LoadScene(3);
            }
        }
    }
}
